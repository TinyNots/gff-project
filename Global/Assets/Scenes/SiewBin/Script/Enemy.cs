using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _sprite;
    private Vector3 _curDest;    //現在の目的地
    bool _isJumping = false;     //跳んでるか
    Rigidbody2D _rb;             
    private StateMachine<Enemy> _stateMachine;   //有限オートマトン
    private Health _health;      //HPの情報
    private float _oldHP;
    private Vector3 _shadowPos;  //立ってる高さ
    private Vector2 _imgSize;
    private float _shadowOffset;     //画像のサイズ
    private bool _dieFlag = false;
    private GetHit _getHitObj;       //攻撃される時の挙動
    [SerializeField]
    private bool _isRanged = false;  //遠攻撃できるか
    [SerializeField]
    private bool _isBoss = false;  //遠攻撃できるか
    private bool _isTargeting = false;
    public GameObject _tmpPlayer;
    public ParticleSystem _particle;
    private bool _targetChangeable;
    private float _chgTargetTime;

    // Start is called before the first frame update
    void Start()
    {
        _imgSize = new Vector2(_sprite.GetComponent<SpriteRenderer>().sprite.texture.width, _sprite.GetComponent<SpriteRenderer>().sprite.texture.height);
        _getHitObj = GetComponent<GetHit>();
        //if (_isRanged)
        //{
        //    _offset.y += 0.2f;
        //}
        _rb = GetComponent<Rigidbody2D>();
        _health = transform.Find("Sprite").gameObject.GetComponent<Health>();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        _stateMachine = new StateMachine<Enemy>();
        _stateMachine.Setup(this, new EnemySpawnDelay());

    }

    // Update is called once per frame
    void Update()
    {
        if ( Time.time > _chgTargetTime + 5)
        {
            _targetChangeable = true;
        }
        //ステートの更新
        _stateMachine.Update();
        if (_dieFlag) return;
        if (_health.ReceiveDmgFlag)
        {
            if (_tmpPlayer != null)
            {
                _tmpPlayer.GetComponent<TargetNum>().TargettedNum--;

            }
            _tmpPlayer = _health.DmgOrigin;
            _tmpPlayer.GetComponent<TargetNum>().TargettedNum++;
            _isTargeting = true;
            _targetChangeable = false;
            _chgTargetTime = Time.time;
            if (_health.HP > 0)
            {
                if (!IsBoss)
                {
                    ChangeState(new EnemyGetHit());
                    return;
                }
            }

        }
        if (_health.HP <= 0)
        {
            _dieFlag = true;
            ChangeState(new EnemyDie());
            return;
        }


        //if (!IsJumping)
        //{
        //    _shadowPos = transform.position - _offset;
        //}
        //else
        //{
        //    _shadowPos = new Vector3(transform.position.x, _shadowPos.y, 0);

        //}
        //if (transform.position.y - _offset.y <= _shadowPos.y)
        //{
        //    IsJumping = false;
        //    _shadowPos = transform.position - _offset;

        //}
        //Debug.DrawLine(new Vector3(transform.position.x, _shadowPos.y, 0), new Vector3(transform.position.x + 2, _shadowPos.y, 0), Color.red);
        //Damage();
        //画像の回転
        if (_isTargeting && !_sprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (GetMoveDir(CurrentDest).x < 0)
            {
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            }
            else
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        //上から生成し、落下させる
        if (_isJumping)
        {
            if (transform.Find("Sprite").transform.position.y -_shadowOffset -0.2f > transform.Find("Shadow").transform.position.y)
            {

                if (transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder < 0)
                {
                    transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
                }
                transform.Find("Sprite").transform.position += transform.TransformDirection(0, -0.2f, 0.0f);
                //何もしない状態
                ChangeState(new EnemySpawnDelay());
            }
            else
            {
                transform.Find("Sprite").transform.position = new Vector3(transform.Find("Sprite").transform.position.x,
                                                                transform.Find("Shadow").transform.position.y + _shadowOffset,0f);
                _isJumping = false;
            }
        }
    }

    private void LateUpdate()
    {
    }
    public Rigidbody2D GetRigidbody()
    {
        return _rb;
    }

    public void ChangeState(IState<Enemy> state)
    {
        _stateMachine.ChangeState(state);
        Debug.Log(_stateMachine.GetCurrentState);
    }

    public bool IsJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
    }

    public Vector3 CurrentDest
    {
        get { return _curDest; }
        set { _curDest = value; }
    }

    public Vector3 ShadowPos
    {
        get { return _shadowPos; }
        set { _shadowPos = value; }
    }

    public float ShadowOffSet
    {
        set { _shadowOffset = value; }
        get { return _shadowOffset; }
    }

    public Vector2 ImgOffSet
    {
        get { return _imgSize; }
    }

    public GameObject Sprite
    {
        get { return _sprite; }
    }

    public GetHit GetHitObj
    {
        get { return _getHitObj; }
    }

    public bool IsRanged
    {
        get { return _isRanged; }
    }

    public bool IsBoss
    {
        get { return _isBoss; }
    }

    public bool IsTargeting
    {
        get { return _isTargeting; }
        set { _isTargeting = value; }
    }

    public bool TargetChangeable
    {
        get { return _targetChangeable; }
        set { _targetChangeable = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //移動してる方向
    public Vector3 GetMoveDir(Vector3 dest)
    {
        var heading = dest - transform.position;
        var direction = heading.normalized; // This is now the normalized direction.
        direction.x = direction.x >= 0f ? 1f : -1f;
        direction.y = direction.y >= 0f ? 1f : -1f;

        return direction;
    }

    //最近くのプレイヤー
    public GameObject FindClosestPlayer()
    {
        if (!_targetChangeable)
        {
            return _tmpPlayer;
        }
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Health>()._hp <= 0) continue;
            Vector3 diff = player.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = player;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject FindRandomPlayer()
    {
        if (!_targetChangeable)
        {
            return _tmpPlayer;
        }
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        int idx = Random.Range(0, PlayerManager._playerTotalIndex + 1);
        return players[idx];
    }

    private void Damage()
    {
        if (Input.anyKeyDown)
        {
            ChangeState(new EnemyGetHit());
        }
    }

 

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
