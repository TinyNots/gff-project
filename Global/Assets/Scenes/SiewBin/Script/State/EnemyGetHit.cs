using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float hitTime;
    private float hitLen;
    public void Enter(Enemy enemy)
    {
        hitTime = Time.time;
        hitLen = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    public void Execute(Enemy enemy)
    {
        if (Time.time > hitTime + hitLen)
        {
            enemy.ChangeState(new EnemyPatrol());
            return;
        }
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
