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
    private Health _health;             //HPの情報
    private Vector3 _shadowPos;         //立ってる高さ
    private float _shadowOffset;        //画像のサイズ
    private bool _dieFlag = false;      //死んだか
    private GetHit _getHitObj;          //攻撃される時の挙動
    [SerializeField]
    private bool _isRanged = false;     //遠攻撃できるか
    [SerializeField]
    private bool _isBoss = false;       //遠攻撃できるか
    private bool _isTargeting = false;  //プレイヤーにターゲットしているか
    public GameObject _tmpPlayer;       //ターゲットしているプレイヤ
    public ParticleSystem _particle;    //ボース専用パーティクル
    private bool _targetChangeable;     //一定時間経ったらターゲットが変えられる
    private float _chgTargetTime;       //ターゲットが変えなくなる時
    private const float unchangeable_time = 5f;  //ターゲットが変えられる必要時間
    private bool _isRetreat = false;    //遠距離敵が撤退するか
    public float _maxTargetNum = 3;     //プレイヤ－がターゲットされる最大数
    private BossSpawnSpot _bossSpot;    //ボースの立つ場所
    [SerializeField]
    //プレイヤーに攻撃されてもEnemyGetHitのステートに入らない
    //攻撃は中断できない
    private bool _unstunnable = false; 
    [SerializeField]
    private bool _multiAttackPattern = false;  //複数の攻撃パターンがあるか
    private Vector2 _colliderBox;           
    [SerializeField]
    private float _attDelay = 1;               //次の攻撃までの待つ時間
    [SerializeField]
    private float _moveSpeed = 5;              //移動速度


    // Start is called before the first frame update
    void Start()
    {
        _sprite = transform.Find("Sprite").gameObject;
        _getHitObj = GetComponent<GetHit>();
        _rb = GetComponent<Rigidbody2D>();
        _health = _sprite.GetComponent<Health>();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        _stateMachine = new StateMachine<Enemy>();
        _stateMachine.Setup(this, new EnemySpawnDelay());
        if (_multiAttackPattern)
        {
            _sprite.GetComponent<Animator>().SetBool("Multi Attack Pattern", true);
        }
        if (_unstunnable)
        {
            _sprite.GetComponent<Animator>().SetBool("Unstunnable", true);
        }
        _colliderBox = _sprite.GetComponent<Collider2D>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.HP <= 0)
        {
            _dieFlag = true;
            Sprite.GetComponent<Animator>().SetTrigger("Dying");
            ChangeState(new EnemyDie());
            return;
        }
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        if (GetComponent<Character>().enabled == false && (transform.position.x > -wsize.x && transform.position.x < wsize.x))
        {
            GetComponent<Character>().enabled = true;
        }

        if ( Time.time > _chgTargetTime + unchangeable_time)
        {
            _targetChangeable = true;
        }
        //ステートの更新
        _stateMachine.Update();
        if (_dieFlag) return;
        if (_health.ReceiveDmgFlag)
        {
            //攻撃された時、攻撃した相手を狙う
            if (_health.DmgOrigin.transform.parent.tag == "Player")
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
            }
            if (_health.HP > 0)
            {
                if (!IsBoss && !_unstunnable)
                {
                    ChangeState(new EnemyGetHit());
                    return;
                }
            }

        }
       
        //画像の回転
        if (_isTargeting && !_sprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
             !_sprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            if (!_isBoss || !IsRanged)
            {
                if (!_isRetreat)
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
                else
                {
                    if (GetMoveDir(CurrentDest).x < 0)
                    {
                        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                }
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

    public bool IsRetreat
    {
        get { return _isRetreat; }
        set { _isRetreat = value; }
    }

    public bool TargetChangeable
    {
        get { return _targetChangeable; }
        set { _targetChangeable = value; }
    }

    public BossSpawnSpot BossSpot
    {
        get { return _bossSpot; }
        set { _bossSpot = value; }
    }

    public bool MultiAttackPattern
    {
        get { return _multiAttackPattern; }
        set { _multiAttackPattern = value; }
    }

    public Vector2 ColliderBox
    {
        get { return _colliderBox; }
    }

    public float AttDelay
    {
        get { return _attDelay; }
    }
    public float MoveSpeed
    {
        get { return _moveSpeed; }
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

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
