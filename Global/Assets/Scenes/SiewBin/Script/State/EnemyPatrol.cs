﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrol : IState<Enemy>
{
    float _destY;
    private float _selfDepth;
    private float _targetDepth;
    private bool _waitFlag;
    private Vector2 _patrolMag = new Vector2(5.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        _destY = wsize.y;

    }

    public void Enter(Enemy enemy)
    {
        enemy.Sprite.GetComponent<Animator>().SetBool("Running", true);
        enemy._tmpPlayer = enemy.FindClosestPlayer();
        if(enemy._tmpPlayer == null)
        {
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
            enemy.ChangeState(new EnemySpawnDelay());
        }
        if (!enemy.IsTargeting && enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum < 5)
        {
            enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum++;
            enemy.IsTargeting = true;
        }
    }

    public void Execute(Enemy enemy)
    {
        var tmp = enemy.FindClosestPlayer();
        if (tmp == null)
        {
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
            enemy.ChangeState(new EnemySpawnDelay());
        }
        if (tmp.GetComponent<TargetNum>().TargettedNum <5 && !enemy.IsTargeting)
        {
            if (tmp != enemy._tmpPlayer)
            {
                enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum--;
                enemy._tmpPlayer = tmp;

            }
            tmp.GetComponent<TargetNum>().TargettedNum++;
            enemy.IsTargeting = true;

        }
        enemy.CurrentDest = enemy._tmpPlayer.transform.position;

        _selfDepth = enemy.GetComponentInChildren<Depth>().DepthSetting;

        _targetDepth = enemy.FindClosestPlayer().transform.parent.GetComponentInChildren<Depth>().DepthSetting;
        if (enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum >=5 && !enemy.IsTargeting)
        {
            Patrol(enemy);
        }
        if (enemy.IsTargeting)
        {

            if (enemy.IsBoss)
            {
                BossChase(enemy);

            }
            else
            {
                if (enemy.IsRanged)
                {
                    RangedChase(enemy);
                    Debug.Log("Ranged Att");
                }
                else
                {
                    MeleeChase(enemy);
                    Debug.Log("Melee Att");
                }
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

    void MeleeChase(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 1.0f)
        {
            if (Mathf.Abs(_selfDepth - _targetDepth) < 0.2f)
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
            if (Mathf.Abs(_selfDepth - _targetDepth) > 0.2f)
            {
                var heading = _targetDepth - _selfDepth;
                heading = heading >= 0f ? 1f : -1f;

                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 5.0f * 0.7f, 0.0f) * Time.deltaTime;
            }
        }
    
    }
    
    void RangedChase(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する

        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (enemy.transform.position.x > -wsize.x && enemy.transform.position.x < wsize.x )
        {
            if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) >1.0f && Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 7.0f)
            {
                if (Mathf.Abs(_selfDepth - _targetDepth) < 0.2f)
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
            if (Mathf.Abs(_selfDepth - _targetDepth) > 0.2f)
            {
                var heading = _targetDepth - _selfDepth;
                heading = heading >= 0f ? 1f : -1f;
                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 5.0f * 0.7f, 0.0f) * Time.deltaTime;
            }
        }
    }

    void BossChase(Enemy enemy)
    {
        enemy.transform.position += enemy.transform.TransformDirection(5.0f, 0.0f, 0.0f) * Time.deltaTime;
        if (enemy.transform.position.x > -0.2f  && enemy.transform.position.x <0.2f)
        {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
        }

    }

    void Patrol(Enemy enemy)
    {
        Collider[] cols = Physics.OverlapSphere(enemy.transform.position, 0.5f);
        foreach ( Collider col in cols)
        {
            if (col.transform.tag == "Player")
            {
                if (enemy.IsRanged)
                {
                    RangedChase(enemy);
                    
                    Debug.Log("Ranged Att");
                }
                else
                {
                    MeleeChase(enemy);
                    Debug.Log("Melee Att");
                }
                return;
            }
              

            
        }

        Stop(enemy);

        enemy.transform.position += enemy.transform.TransformDirection(_patrolMag.x, _patrolMag.y * 0.7f, 0.0f) * Time.deltaTime;
    }

    public void Stop(Enemy enemy)
    {
        if (_waitFlag)
        {
            return;
        }
        StaticCoroutine.StartCoroutine(Wait(enemy));
    }

    private IEnumerator Wait( Enemy enemy)
    {

        _waitFlag = true;
        yield return new WaitForSeconds(1f);
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        var val = Random.Range(0, 2);
        if (val == 1)
        {
            _patrolMag.x = 5f;
            
            enemy.transform.Rotate(new Vector3(0f, 180f, 0f));
            //if (enemy.transform.position.x < -wsize.x / 2)
            //{
            //    enemy.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            //}
            //else if(enemy.transform.position.x > wsize.x / 2)
            //{
            //    enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            //}
            val = Random.Range(0, 6);
            
            if (val == 1)
            {
                if (enemy.transform.position.y > 0f)
                {
                    _patrolMag.y = -5.0f;
                }
                else
                {
                    _patrolMag.y = 5.0f;
                }
            }
            else
            {
                _patrolMag.y = 0f;
            }
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", true);

        }
        if (val == 0)
        {
            _patrolMag.x = 0;
            _patrolMag.y = 0;
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);

        }
        if (enemy.transform.position.x < -wsize.x / 2)
        {
            _patrolMag.x = 5f;

            enemy.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", true);

        }
        else if (enemy.transform.position.x > wsize.x / 2)
        {
            _patrolMag.x = 5f;

            enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            enemy.Sprite.GetComponent<Animator>().SetBool("Running", true);


        }
        _waitFlag = false;
    }

   
}
