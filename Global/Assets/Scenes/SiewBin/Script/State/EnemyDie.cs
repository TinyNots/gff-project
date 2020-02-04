﻿using UnityEngine;

public class EnemyDie : IState<Enemy>
{
    float _dieTime = 0;
    private Animator _animator;

    public void Enter(Enemy enemy)
    {
        enemy.Sprite.GetComponent<BoxCollider2D>().enabled = false;
        enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum--;
        enemy.IsTargeting = false;
        _dieTime = Time.time;

        _animator = enemy.Sprite.GetComponent<Animator>();
        _animator.SetTrigger("Dying");
    }

    public void Execute(Enemy enemy)
    {
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            _animator.SetTrigger("Dying");
        }
        //var anim = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        //if (Time.time > _dieTime + anim.length)
        //{

        //    Debug.Log("Destroy enemy");

        //    EnemyEvents events = enemy.transform.Find("Sprite").GetComponent<EnemyEvents>();
        //    if(events != null)
        //    {
        //        events.SpawnCoin(4, 5);
        //    }

        //    enemy.DestroySelf();
        //}
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
