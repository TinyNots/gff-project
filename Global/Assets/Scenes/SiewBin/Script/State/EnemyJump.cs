using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : IState<Enemy>
{
    int jumpCnt = 0;
    public void Enter(Enemy enemy)
    {
    }

    public void Execute(Enemy enemy)
    {
        Vector3 destX = new Vector3(enemy.dest[enemy.CurrentDest].x, enemy.transform.position.y);
        if (!enemy.IsJumping() && enemy.GetRigidbody().velocity == new Vector2(0,0))
        {
            if (Vector3.Distance(enemy.transform.position, destX) < 0.5f)
            {
                enemy.CurrentDest = (enemy.CurrentDest + 1) % enemy.dest.Length;
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            enemy.GetRigidbody().AddForce(new Vector2(enemy.GetMoveDir().x * 3.0f, 3.0f), ForceMode2D.Impulse);

            Debug.Log(enemy.GetRigidbody().velocity);
        }
        
        //enemy.ChangeState(new EnemyPatrol());

    }
    public void Exit(Enemy enemy)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
