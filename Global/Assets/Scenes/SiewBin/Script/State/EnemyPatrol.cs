using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : IState<Enemy>
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Enter(Enemy enemy)
    {

    }

    public void Execute(Enemy enemy)
    {
        Vector3 destX = new Vector3(enemy.dest[enemy.CurrentDest].x, enemy.transform.position.y);
      enemy.transform.position += enemy.transform.TransformDirection(enemy.GetMoveDir().x *0.1f, 0.0f, 0.0f);
      if (Vector3.Distance(enemy.transform.position, destX) < 0.5f)
      {
           enemy.CurrentDest = (enemy.CurrentDest + 1) % enemy.dest.Length;
           Debug.Log("ChangeToJump");
            if (Random.Range(0, 2) == 0)
            { 
                enemy.ChangeState(new EnemyJump());
            }
      }

    }
    public void Exit(Enemy enemy)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveState()
    {

    }



}
