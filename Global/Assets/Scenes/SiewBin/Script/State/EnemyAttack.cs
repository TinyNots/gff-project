using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<Enemy>
{

    float attTime = 1;
    private float selfDepth;
    private float targetDepth;
    private AnimationClip anim;


    private float particleStartTime;
    private bool startParticleFlag;

    public void Enter(Enemy enemy)
    {
        //攻撃アニメ
        anim = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        enemy.Sprite.GetComponent<Animator>().Play("Idle");
        selfDepth = enemy.GetComponentInChildren<Depth>().DepthSetting;
        particleStartTime = Time.time;
        startParticleFlag = false;

    }
    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;
        targetDepth = enemy.FindClosestPlayer().transform.parent.GetComponentInChildren<Depth>().DepthSetting;

        if (enemy.IsBoss)
        {
            BossAttack(enemy);

        }
        else
        {
            if (enemy.IsRanged)
            {
                RangedAttack(enemy);

            }
            else
            {
                MeleeAttack(enemy);
            }
        }
    }

    

    public void Exit(Enemy enemy)
    {
        enemy.Sprite.GetComponent<BoxCollider2D>().isTrigger = false;
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
        var distY = selfDepth - targetDepth;


        if (Time.time > attTime + anim.length+1)
        {
            //目標が攻撃範囲から離れた
            if (Mathf.Abs(distX) > 1.0f || Mathf.Abs(distY) > 0.2f)
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
        }
    }

    void RangedAttack (Enemy enemy)
    {
        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = enemy.transform.position.y - enemy.CurrentDest.y;
        if (Time.time > attTime + anim.length+2)
        {
            if (Mathf.Abs(distX) >7.5f || Mathf.Abs(distY) > 0.3f)
            {
                //目標が攻撃範囲から離れた
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
        }
    }

    void BossAttack(Enemy enemy)
    {
        if (Time.time > attTime + anim.length + 2)
        {
            if (!startParticleFlag)
            {
                particleStartTime = Time.time;
            }
            startParticleFlag = true;

            if (startParticleFlag)
            {
                enemy.particle.gameObject.SetActive(true);
                if (particleStartTime + 5 < Time.time)
                {
                    enemy.particle.Stop();
                    //startParticleFlag = false;
                    Debug.Log("Attack");
                    //enemy.GetComponent<BoxCollider2D>().isTrigger = true;
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");
                    attTime = Time.time;
                    startParticleFlag = false;
                    enemy.particle.gameObject.SetActive(false);

                }
            }
          
        }
     
    }
}
