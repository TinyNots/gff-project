using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : IState<Enemy>
{
    static int destPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Enter(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.dest[destPoint]) < 0.5f)
        {
            destPoint++;
        }
    }

    public void Execute(Enemy enemy)
    {
      enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.dest[destPoint], 5f * Time.deltaTime);
      if (Vector3.Distance(enemy.transform.position, enemy.dest[destPoint]) < 0.5f)
      {
           destPoint++;
           enemy.ChangeState(new EnemyJump());
      }
      destPoint = destPoint % enemy.dest.Length;
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
