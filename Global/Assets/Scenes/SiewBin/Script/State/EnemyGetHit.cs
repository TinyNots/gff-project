using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float _hitTime;
    private float _hitLen;
    private float _tiltLen;
    public void Enter(Enemy enemy)
    {

        enemy.Sprite.GetComponent<Animator>().SetTrigger("Hit");
        _hitTime = Time.time;
        _hitLen = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;

        _tiltLen = 2;
        enemy.GetHitObj.GetHitInit(_hitLen);
        if (enemy.IsRanged && !enemy.IsRetreat)
        {
            enemy.IsRetreat = true;
        }

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
