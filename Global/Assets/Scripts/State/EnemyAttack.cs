using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : IState<Enemy>
{
    private const float screen_width_offset = 1f;
    private const float particle_charge_time = 5f;
    float _attTime = 1;
    private float _selfDepth;
    private float _targetDepth;
    private AnimationClip _anim;


    private float _particleStartTime;
    private bool _startParticleFlag;

    public void Enter(Enemy enemy)
    {
        //攻撃アニメ
        enemy.Sprite.GetComponent<Animator>().Play("Idle");
        _selfDepth = enemy.GetComponentInChildren<Depth>().DepthSetting;
        _particleStartTime = Time.time;
        _startParticleFlag = false;
        _anim = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;

    }
    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;
        _targetDepth = enemy.FindClosestPlayer().transform.parent.GetComponentInChildren<Depth>().DepthSetting;

        if (enemy.IsBoss)
        {
            if (enemy.IsRanged)
            {
                BossRangedAttack(enemy);
            }
            else
            {
                BossMeleeAttack(enemy);
            }
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
        _anim = enemy.Sprite.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;

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
        var distY = _selfDepth - _targetDepth;


        if (Time.time > _attTime + _anim.length + enemy.AttDelay)
        {
            //目標が攻撃範囲から離れた
            if (Mathf.Abs(distX) > enemy.ColliderBox.x || Mathf.Abs(distY) > 0.4f)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Debug.Log("ChangeToPatrol");
                    enemy.ChangeState(new EnemyPatrol());
                    return;
                }
            }
            if (Mathf.Abs(distX) > enemy.ColliderBox.x  + 2.0f || Mathf.Abs(distY) > 2.0f)
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //複数攻撃パターンの検査
            if (enemy.MultiAttackPattern)
            {
                if (Random.Range(0, 2) == 0)
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

                }
                else
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack 2");


                }
            }
            else
            {
                enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

            }
            _attTime = Time.time;
        }
    }

    void RangedAttack(Enemy enemy)
    {
        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = enemy.transform.position.y - enemy.CurrentDest.y;
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        //次の行動時間
        if (Time.time > _attTime + _anim.length + enemy.AttDelay)
        {
            //攻撃範囲内
            if (Mathf.Abs(distX) > 7.5f || 
                Mathf.Abs(distY) > 0.5f)
            {
                //そのままも一度攻撃か、プレイヤーに追いかけるか
                if (Random.Range(0, 2) == 0)
                {
                    //目標が攻撃範囲から離れた
                    Debug.Log("ChangeToPatrol");
                    enemy.ChangeState(new EnemyPatrol());
                    return;
                }
            }
            if (enemy.transform.position.x > -wsize.x + screen_width_offset &&
                enemy.transform.position.x <  wsize.x - screen_width_offset)
            {
                //プレイヤーが迫って来る
                if (Mathf.Abs(distX) < 5.0f && 
                    Mathf.Abs(distY) < 0.2f)
                {
                    //退避処理する
                    Debug.Log("ChangeToPatrol");
                    enemy.ChangeState(new EnemyPatrol());
                    enemy.IsRetreat = true;
                    return;
                }
            }
            Debug.Log("Attack");
            //複数攻撃パターンの検査
            if (enemy.MultiAttackPattern)
            {
                if (Random.Range(0, 2) == 0)
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

                }
                else
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack 2");


                }
            }
            else
            {
                enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

            }
            _attTime = Time.time;

        }
    }

    void BossRangedAttack(Enemy enemy)
    {
        if (Time.time > _attTime + _anim.length + enemy.AttDelay)
        {
            //パーティクルチャージ
            if (!_startParticleFlag)
            {
                _particleStartTime = Time.time;
            }
            _startParticleFlag = true;

            if (_startParticleFlag)
            {
                enemy._particle.gameObject.SetActive(true);
                if (Time.time > _particleStartTime + particle_charge_time)
                {
                    //チャージ終了
                    //攻撃開始
                    enemy._particle.Stop();
                    Debug.Log("Attack");
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");
                    _attTime = Time.time;
                    _startParticleFlag = false;
                    enemy._particle.gameObject.SetActive(false);

                }
            }

        }

    }

    void BossMeleeAttack(Enemy enemy)
    {
        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = _selfDepth - _targetDepth;

        if (Time.time > _attTime + _anim.length + enemy.AttDelay)
        {
            if (Mathf.Abs(distX) > enemy.ColliderBox.x + 5.0f )
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            //複数攻撃パターンの検査
            if (enemy.MultiAttackPattern)
            {
                if (Random.Range(0, 2) == 0)
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

                }
                else
                {
                    enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack 2");


                }
            }
            else
            {
                enemy.Sprite.GetComponent<Animator>().SetTrigger("Attack");

            }
            _attTime = Time.time;
        }
    }
}
