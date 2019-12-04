using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    private Vector3 curDest;
    bool isJumping = false;
    Rigidbody2D rb;
    public Vector3[] dest;
    private StateMachine<Enemy> stateMachine;
    private Health health;

    private Vector3 shadowPos;
    private Vector3 offset;
    [SerializeField]
    private GameObject attackBox;
    private GameObject tmpSlash;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        offset = new Vector3(GetComponent<BoxCollider2D>().size.x , GetComponent<BoxCollider2D>().size.y , 0);
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        dest[0]  = new Vector3(wsize.x, transform.position.y,0);
        dest[1] = new Vector3(-8, transform.position.y, 0);
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyPatrol());
        shadowPos = transform.position - offset;

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        if (!IsJumping)
        {
            shadowPos = transform.position - offset;
        }
        else
        {
            shadowPos = new Vector3(transform.position.x, shadowPos.y, 0);
        }
        if (transform.position.y - offset.y <= shadowPos.y)
        {
            IsJumping = false;
            shadowPos = transform.position - offset;

        }
        Debug.DrawLine(new Vector3(transform.position.x, shadowPos.y, 0), new Vector3(transform.position.x + 2, shadowPos.y, 0), Color.red);
        Damage();
        if (GetMoveDir(CurrentDest).x < 0)
        {
           transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else
        {
           transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
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
    public Vector3 OffSet
    {
        get { return offset; }
    }

    public GameObject AttackBox
    {
        get { return attackBox; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().ReceiveDmg(10);
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health.ReceiveDmg(10);
    }

    public Vector3 GetMoveDir(Vector3 dest)
    {
        var heading = dest - transform.position;
        var direction = heading.normalized; // This is now the normalized direction.
        direction.x = direction.x >= 0f ? 1f : -1f;
        direction.y = direction.y >= 0f ? 1f : -1f;

        return direction;
    }
     public GameObject FindClosestPlayer()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject player in players)
        {
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
            ChangeState(new EnemyDie());
        }
    }

    void SpawnAttack()
    {
        tmpSlash = Instantiate(AttackBox, transform);
        tmpSlash.SetActive(true);
    }

    void ResetAttack()
    {
        Destroy(tmpSlash);
    }

    public void DestroySelf()
    {
        Destroy(this);
    }
}
