using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject sprite;
    private Vector3 curDest;    //現在の目的地
    bool isJumping = false;     //跳んでるか
    Rigidbody2D rb;             
    public Vector3[] dest;      
    private StateMachine<Enemy> stateMachine;   //有限オートマトン
    private Health health;      //HPの情報
    private float oldHP;
    private Vector3 shadowPos;  //立ってる高さ
    private float offset;     //画像のサイズ
    private bool dieFlag = false;
    private GetHit getHitObj;       //攻撃される時の挙動
    [SerializeField]
    private bool isRanged = false;  //遠攻撃できるか
    [SerializeField]
    private bool isBoss = false;  //遠攻撃できるか
    private bool isTargeting = false;
    public GameObject tmpPlayer;
    public ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {

        getHitObj = GetComponent<GetHit>();
        //if (isRanged)
        //{
        //    offset.y += 0.2f;
        //}
        rb = GetComponent<Rigidbody2D>();
        health = transform.Find("Sprite").gameObject.GetComponent<Health>();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        dest[0]  = new Vector3(wsize.x, transform.position.y,0);
        dest[1] = new Vector3(-8, transform.position.y, 0);
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemySpawnDelay());

    }

    // Update is called once per frame
    void Update()
    {
        //ステートの更新
        stateMachine.Update();
        if (dieFlag) return;
        if (health.ReceiveDmgFlag)
        {
            //if ((collision.gameObject.transform.parent.rotation.y % 360) == transform.rotation.y)
            //{
            //    transform.Rotate(new Vector3(0, 180f));
            //}
            if (health.HP > 0)
            {
                ChangeState(new EnemyGetHit());
            }

        }
        if (health.HP <= 0)
        {
            dieFlag = true;
            ChangeState(new EnemyDie());
        }


        //if (!IsJumping)
        //{
        //    shadowPos = transform.position - offset;
        //}
        //else
        //{
        //    shadowPos = new Vector3(transform.position.x, shadowPos.y, 0);

        //}
        //if (transform.position.y - offset.y <= shadowPos.y)
        //{
        //    IsJumping = false;
        //    shadowPos = transform.position - offset;

        //}
        //Debug.DrawLine(new Vector3(transform.position.x, shadowPos.y, 0), new Vector3(transform.position.x + 2, shadowPos.y, 0), Color.red);
        //Damage();
        //画像の回転
        if (isTargeting)
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
        if (isJumping)
        {
            if (transform.Find("Sprite").transform.position.y -offset -0.2f > transform.Find("Shadow").transform.position.y)
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
                                                                transform.Find("Shadow").transform.position.y + offset,0f);
                isJumping = false;
            }
        }
    }

    private void LateUpdate()
    {
    }
    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public void ChangeState(IState<Enemy> state)
    {
        stateMachine.ChangeState(state);
        Debug.Log(stateMachine.GetCurrentState);
    }

    public bool IsJumping
    {
        get { return isJumping; }
        set { isJumping = value; }
    }

    public Vector3 CurrentDest
    {
        get { return curDest; }
        set { curDest = value; }
    }

    public Vector3 ShadowPos
    {
        get { return shadowPos; }
        set { shadowPos = value; }
    }

    public float OffSet
    {
        set { offset = value; }
        get { return offset; }
    }

    

    public GameObject Sprite
    {
        get { return sprite; }
    }

    public GetHit GetHitObj
    {
        get { return getHitObj; }
    }

    public bool IsRanged
    {
        get { return isRanged; }
    }

    public bool IsBoss
    {
        get { return isBoss; }
    }

    public bool IsTargeting
    {
        get { return isTargeting; }
        set { isTargeting = value; }

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
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Health>().hp <= 0) continue;
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
