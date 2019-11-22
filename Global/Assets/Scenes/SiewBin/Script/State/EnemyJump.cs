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
        if (!enemy.IsJumping())
        {
            enemy.GetRigidbody().AddForce(new Vector3(0.0f, 5.0f, 0.0f), ForceMode2D.Impulse);
            jumpCnt++;
        }
      
            enemy.ChangeState(new EnemyPatrol());
        
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
