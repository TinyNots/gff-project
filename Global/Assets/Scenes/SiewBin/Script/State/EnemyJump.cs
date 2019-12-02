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
        Vector3 destX = new Vector3(enemy.dest[enemy.CurrentDest].x, enemy.transform.position.y);
        if (!enemy.IsJumping)
        {
            if (Vector3.Distance(enemy.transform.position, destX) < 0.5f)
            {
                enemy.CurrentDest = (enemy.CurrentDest + 1) % enemy.dest.Length;
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log(_currentVelocity);
            enemy.IsJumping = true;
            _currentVelocity = _jumpVelocity;
            //enemy.transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);
   
        }
        else
        {
            _currentVelocity -= 10f * Time.deltaTime;
        }
        enemy.transform.position += enemy.transform.TransformDirection(enemy.GetMoveDir(enemy.dest[enemy.CurrentDest]).x * 0.05f, 0.5f * _currentVelocity * Time.deltaTime, 0.0f);

        //enemy.transform.position += enemy.transform.TransformDirection( 0.0f, 0.1f, 0.0f);
        
        //}
        
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
