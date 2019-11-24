using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;
    private int curDest = 0;
    bool isJumping = true;
    Rigidbody2D rb;
    public Vector3[] dest;
    private StateMachine<Enemy> stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));
        dest[0]  = new Vector3(wsize.x, transform.position.y,0);
        dest[1] = new Vector3(-8, transform.position.y, 0);
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyPatrol());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
    
    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public void ChangeState(IState<Enemy> state)
    {
        stateMachine.ChangeState(state);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isJumping = true;
        }
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public int CurrentDest
    {
        get { return curDest; }
        set { curDest = value; }
    }

    public Vector3 GetMoveDir()
    {
        var heading = dest[CurrentDest] - transform.position;
        var direction = heading.normalized; // This is now the normalized direction.
        direction.x = direction.x >= 0f ? 1f : -1f;
        direction.y = direction.y >= 0f ? 1f : -1f;


        return direction;
    }
}
