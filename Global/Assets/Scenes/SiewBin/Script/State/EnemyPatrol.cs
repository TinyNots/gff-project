using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrol : IState<Enemy>
{
    float destY;
    private float selfDepth;
    private float targetDepth;
    // Start is called before the first frame update
    void Start()
    {
        
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        destY = wsize.y;

    }

    public void Enter(Enemy enemy)
    {
        enemy.Sprite.GetComponent<Animator>().SetBool("Running", true);
    }

    public void Execute(Enemy enemy)
    {
        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;
        selfDepth = enemy.GetComponentInChildren<Depth>().DepthSetting;

        targetDepth = enemy.FindClosestPlayer().transform.parent.GetComponentInChildren<Depth>().DepthSetting;
        if (enemy.IsRanged)
        {
            RangedPatrol(enemy);
            Debug.Log("Ranged Att");
        }
        else
        {
            MeleePatrol(enemy);
            Debug.Log("Melee Att");
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

    void MeleePatrol(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 1.0f)
        {
            if (Mathf.Abs(selfDepth - targetDepth) < 0.2f)
            {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
            }
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > 1.0f)
        {
            enemy.transform.position += enemy.transform.TransformDirection(5.0f, 0.0f, 0.0f) * Time.deltaTime;
        }
        
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 4.0f)
        {
            if (Mathf.Abs(selfDepth - targetDepth) > 0.2f)
            {
                var heading = targetDepth - selfDepth;
                heading = heading >= 0f ? 1f : -1f;

                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 5.0f * 0.7f, 0.0f) * Time.deltaTime;
            }
        }
    
    }
    
    void RangedPatrol(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する

        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (enemy.transform.position.x > -wsize.x && enemy.transform.position.x < wsize.x )
        {
            if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) >3.0f && Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 7.0f)
            {
                if (Mathf.Abs(selfDepth - targetDepth) < 0.2f)
                {
                    enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                    enemy.ChangeState(new EnemyAttack());
                    return;
                }
            }
        }
        //スクリーン内に見えるまで移動
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > 6.0f ||
            !(enemy.transform.position.x > -wsize.x  && enemy.transform.position.x < wsize.x))
        {
            enemy.transform.position += enemy.transform.TransformDirection(5.0f, 0.0f, 0.0f) * Time.deltaTime;
        }
        //if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 3.0f)
        //{
        //    enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        //    enemy.transform.position += enemy.transform.TransformDirection(0.1f, 0.0f, 0.0f);

        //}
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 9.0f)
        {
            if (Mathf.Abs(selfDepth - targetDepth) > 0.2f)
            {
                var heading = targetDepth - selfDepth;
                heading = heading >= 0f ? 1f : -1f;
                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 5.0f * 0.7f, 0.0f) * Time.deltaTime;
            }
        }
    }


}
