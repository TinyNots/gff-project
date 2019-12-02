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


        Vector3 playerPos = enemy.FindClosestPlayer().transform.position;
        
   
        
        var anim = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        if (Time.time > attTime + anim.length)
        {
            Debug.Log("Attack");
            Debug.Log(anim.name);
            enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.GetComponent<Animator>().SetTrigger("Attack");
            attTime = Time.time;
        }



        var distY = enemy.transform.position.y - playerPos.y;
 
        if (Vector3.Distance(enemy.transform.position, playerPos) >2.0f && Mathf.Abs(distY) > 0.5f)
        {

            Debug.Log("ChangeToPatrol");
            enemy.ChangeState(new EnemyPatrol());
            return;
            
        }

        //if (Vector3.Distance(enemy.transform.position, destX) < 0.5f)
        //{
        //   enemy.CurrentDest = (enemy.CurrentDest + 1) % enemy.dest.Length;
        //   Debug.Log("ChangeToJump");
        //    if (Random.Range(0, 2) == 0)
        //    { 
        //       enemy.ChangeState(new EnemyJump());
        //    }
        //}

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
