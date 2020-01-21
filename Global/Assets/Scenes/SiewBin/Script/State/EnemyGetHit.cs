using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float hitTime;
    private float hitLen;
    private float tiltLen;
    public void Enter(Enemy enemy)
    {

        enemy.Sprite.GetComponent<Animator>().SetTrigger("Hit");
        hitTime = Time.time;
        hitLen = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;

        tiltLen = 2;
        enemy.GetHitObj.GetHitInit(hitLen);

    }

    public void Execute(Enemy enemy)
    {
        if (enemy.GetHitObj.Moveable)
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
