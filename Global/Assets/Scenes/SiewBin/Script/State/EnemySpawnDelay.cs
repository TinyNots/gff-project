using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDelay : IState<Enemy>
{
    float waitTime = 1;
    float timeStart;

    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        timeStart = Time.time;
    }

    public void Execute(Enemy enemy)
    {
        if (Time.time > timeStart + waitTime)
        {
            enemy.ChangeState(new EnemyPatrol());
        }
    }

    public void Exit(Enemy enemy)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
