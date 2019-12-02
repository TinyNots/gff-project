using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<Enemy>
{
    float attTime = 0;
    public void Enter(Enemy enemy)
    {
    }

    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;

        var distX = enemy.transform.position.x - enemy.CurrentDest.x;
        var distY = enemy.transform.position.y - enemy.CurrentDest.y;


        var anim = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        if (Time.time > attTime + anim.length)
        {
            if (Mathf.Abs(distX) > 0.5f && Mathf.Abs(distY) > 0.3f)
            {
                Debug.Log("ChangeToPatrol");
                enemy.ChangeState(new EnemyPatrol());
                return;
            }
            Debug.Log("Attack");
            enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
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
}
