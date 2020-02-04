using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrol : IState<Enemy>
{
    float _destY;
    private float _selfDepth;
    private float _targetDepth;
    private bool _waitFlag;
    private Vector2 _patrolMag = new Vector2(5.0f, 0.0f);
    private bool _patrolFlag;
    private float patrolCnt;

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
        if (!enemy.IsTargeting && enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum < enemy._maxTargetNum)
        {
            enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum++;
            enemy.IsTargeting = true;
        }
    }

    public void Execute(Enemy enemy)
    {
        if (patrolCnt > 120)
        {
            _patrolFlag = false;
        }
        if (!enemy.IsTargeting)
        {
            if (Random.Range(0, 3) == 0)
            {
                _patrolFlag = true;
            }
        }
        
        if (!_patrolFlag)
        {
            var tmp = enemy.FindClosestPlayer();
            if (tmp == null)
            {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemySpawnDelay());
            }
            if (tmp.GetComponent<TargetNum>().TargettedNum < enemy._maxTargetNum && !enemy.IsTargeting)
            {
                if (tmp != enemy._tmpPlayer)
                {
                    enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum--;
                    enemy._tmpPlayer = tmp;

                }
                tmp.GetComponent<TargetNum>().TargettedNum++;
                enemy.IsTargeting = true;

            }
            if (enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum > enemy._maxTargetNum && enemy.IsTargeting)
            {
                if (Random.Range(0, 2) == 0)
                {
                    enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum--;
                    enemy.IsTargeting = false;
                }
            }
            enemy.CurrentDest = enemy._tmpPlayer.transform.position;

            _selfDepth = enemy.GetComponentInChildren<Depth>().DepthSetting;

            _targetDepth = enemy.FindClosestPlayer().transform.parent.GetComponentInChildren<Depth>().DepthSetting;
        }
        else
        {
            Patrol(enemy);
        }
        if (enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum >= enemy._maxTargetNum && !enemy.IsTargeting)
        {
            Patrol(enemy);
        }
        if (enemy.IsTargeting)
        {
            if (enemy.IsBoss)
            {
                if (enemy.IsRanged)
                {
                    BossRangedChase(enemy);
                }
                else
                {
                    BossMeleeChase(enemy);
                }

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


        patrolCnt++;
    }
    
    public void Exit(Enemy enemy)
    {
        enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);

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
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < enemy.ColliderBox.x)
        {
            if (Mathf.Abs(_selfDepth - _targetDepth) < 0.2f)
            {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
            }
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > enemy.ColliderBox.x)
        {
            enemy.transform.position += enemy.transform.TransformDirection(enemy.MoveSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }
        
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 4.0f)
        {
            if (Mathf.Abs(_selfDepth - _targetDepth) > 0.2f)
            {
                var heading = _targetDepth - _selfDepth;
                heading = heading >= 0f ? 1f : -1f;

                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 2.5f, 0.0f) * Time.deltaTime;
            }
        }
    
    }
    
    void RangedChase(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する

        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (enemy.transform.position.x > -wsize.x +1f && enemy.transform.position.x < wsize.x -1f)
        {
            if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) >1.0f && Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 7.0f && !enemy.IsRetreat)
            {
                if (Mathf.Abs(_selfDepth - _targetDepth) < 0.2f)
                {
                    enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                    enemy.ChangeState(new EnemyAttack());
                    enemy.IsRetreat = false;
                    return;
                }
            }
            if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > 5.0f && enemy.IsRetreat)
            {
                    enemy.IsRetreat = false;
            }

        }
        else
        {
            if (enemy.IsRetreat)
            {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                enemy.IsRetreat = false;
                return;
            }
            
        }
        //スクリーン内に見えるまで移動
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > 6.0f ||
            !(enemy.transform.position.x > -wsize.x  && enemy.transform.position.x < wsize.x) ||
            enemy.IsRetreat)
        {
            enemy.transform.position += enemy.transform.TransformDirection(enemy.MoveSpeed, 0.0f, 0.0f) * Time.deltaTime;
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
                enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 2.5f, 0.0f) * Time.deltaTime;
            }
        }
    }

    void BossRangedChase(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        enemy.transform.position += enemy.transform.TransformDirection(enemy.MoveSpeed, 0.0f, 0.0f) * Time.deltaTime;
        bool stateChgFlag = false;
        switch(enemy.BossSpot)
        {
            case BossSpawnSpot.Half:
                if (enemy.transform.position.x > -0.2f && enemy.transform.position.x < 0.2f)
                {
                    stateChgFlag = true;
                }
                break;
            case BossSpawnSpot.OneOverFour:
                if (enemy.transform.position.x > -0.2f - wsize.x / 2 && enemy.transform.position.x < 0.2f - wsize.x / 2)
                {
                    stateChgFlag = true;
                }
                break;
            case BossSpawnSpot.ThreeOverFour:
                if (enemy.transform.position.x > -0.2f + wsize.x / 2 && enemy.transform.position.x < 0.2f + wsize.x / 2)
                {
                    stateChgFlag = true;
                }
                break;
        }
        if (stateChgFlag)
        {
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
        }

    }

    void BossMeleeChase(Enemy enemy)
    {
        //攻撃範囲内だったら攻撃する
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        if (enemy.transform.position.x > -wsize.x && enemy.transform.position.x < wsize.x)
        {
            if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < enemy.ColliderBox.x + 5f)
            {
                //if (Mathf.Abs(_selfDepth - _targetDepth) < 0.2f)
                //{
                enemy.Sprite.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
                //}
            }
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > enemy.ColliderBox.x)
        {
            enemy.transform.position += enemy.transform.TransformDirection(enemy.MoveSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }

        //if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 4.0f)
        //{
        //    if (Mathf.Abs(_selfDepth - _targetDepth) > 0.2f)
        //    {
        //        var heading = _targetDepth - _selfDepth;
        //        heading = heading >= 0f ? 1f : -1f;

        //        enemy.transform.position += enemy.transform.TransformDirection(0.0f, heading * 2.5f, 0.0f) * Time.deltaTime;
        //    }
        //}

    }

    void Patrol(Enemy enemy)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(enemy.transform.position, 0.2f);
        foreach ( Collider2D col in cols)
        {
            if (col.transform.tag == "Player")
            {
                if (col.GetComponent<TargetNum>().TargettedNum < 5)
                {
                    enemy.IsTargeting = true;
                    _patrolFlag = false;
                    enemy._tmpPlayer = col.gameObject;
                    col.GetComponent<TargetNum>().TargettedNum++;
                    if (enemy.IsRanged)
                    {
                        RangedChase(enemy);

                    }
                    else
                    {
                        MeleeChase(enemy);
                    }
                    return;
                }
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
                    _patrolMag.y = -2.0f;
                }
                else
                {
                    _patrolMag.y = 2.0f;
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
