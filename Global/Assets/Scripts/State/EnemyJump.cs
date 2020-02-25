using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : IState<Enemy>
{
    private float _jumpVelocity = 10f;
    private float _currentVelocity = 0;

    public void Enter(Enemy enemy)
    {
    }

    public void Execute(Enemy enemy)
    {
        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;

        if (!enemy.IsJumping)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.CurrentDest) < 0.5f)
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log(_currentVelocity);
            enemy.IsJumping = true;
            _currentVelocity = _jumpVelocity;
   
        }
        else
        {
            _currentVelocity -= 10f * Time.deltaTime;
        }
        enemy.transform.position += enemy.transform.TransformDirection(enemy.GetMoveDir(enemy.CurrentDest).x * 0.05f, 0.5f * _currentVelocity * Time.deltaTime, 0.0f);

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
