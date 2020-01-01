using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrol : IState<Enemy>
{
    float destY;

    // Start is called before the first frame update
    void Start()
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        destY = wsize.y;
        
    }

    public void Enter(Enemy enemy)
    {
        enemy.GetComponent<Animator>().SetBool("Running", true);
    }

    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        enemy.CurrentDest = enemy.FindClosestPlayer().transform.position;
       

        if (Vector3.Distance(enemy.transform.position, enemy.CurrentDest) < 1.0f)
        {
            if (Mathf.Abs(enemy.transform.position.y - enemy.CurrentDest.y) < 0.5f)
            {
                Debug.Log("Enemy Attack");
                enemy.GetComponent<Animator>().SetBool("Running", false);
                enemy.ChangeState(new EnemyAttack());
                return;
            }
            //else
            //{
                
            //     enemy.transform.position += enemy.transform.TransformDirection(0.0f, enemy.GetMoveDir(enemy.CurrentDest).y * 0.05f, 0.0f);
                
            //}
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) > 0.5f)
        {
            enemy.transform.position += enemy.transform.TransformDirection(0.1f, 0.0f, 0.0f);
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.CurrentDest.x) < 6.0f)
        {
            if (Mathf.Abs(enemy.transform.position.y - enemy.CurrentDest.y) > 0.3f)
            {
                enemy.transform.position += enemy.transform.TransformDirection(0.0f, enemy.GetMoveDir(enemy.CurrentDest).y * 0.05f, 0.0f);
            }
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveState()
    {

    }

    


}
