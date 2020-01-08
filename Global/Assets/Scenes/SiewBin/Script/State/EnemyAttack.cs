using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<Enemy>
{

    float attTime = 0;
    private AnimationClip anim;
    public void Enter(Enemy enemy)
    {
        //攻撃アニメ
        anim = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
    }

    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;
        if (enemy.IsRanged)
        {
            RangedAttack(enemy);

        }
        else
        {
            MeleeAttack(enemy);
        }
    
    }

    

    public void Exit(Enemy enemy)
    {
        enemy.GetComponent<BoxCollider2D>().isTrigger = false;

    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MeleeAttack(Enemy enemy)
    {
        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = enemy.transform.position.y - enemy.CurrentDest.y;

      
        if (Time.time > attTime + anim.length+1)
        {
            //目標が攻撃範囲から離れた
            if (Mathf.Abs(distX) > 1.0f || Mathf.Abs(distY) > 0.3f)
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
        }
    }

    void RangedAttack (Enemy enemy)
    {
        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = enemy.transform.position.y - enemy.CurrentDest.y;

        if (Time.time > attTime + anim.length+1)
        {
            if (Mathf.Abs(distX) > 7.0f || Mathf.Abs(distY) > 0.5f)
            {
                //目標が攻撃範囲から離れた
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
        }
    }
}
