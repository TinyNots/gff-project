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

    }

    public void Execute(Enemy enemy)
    {
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        Vector3 destX = new Vector3(enemy.dest[enemy.CurrentDest].x, destY);
        Vector3 dest = new Vector3(enemy.transform.position.x, destY);

        enemy.transform.position += enemy.transform.TransformDirection(enemy.GetMoveDir(enemy.dest[enemy.CurrentDest]).x *0.05f, 0.0f, 0.0f);
        enemy.transform.position += enemy.transform.TransformDirection(0.0f, enemy.GetMoveDir(dest).y * 0.1f, 0.0f);

        if (Vector3.Distance(enemy.transform.position, dest) < 0.5f)
        {
            if (dest.y > 0)
            {
                destY = -4.8f;
            }
            else
            {
                destY = wsize.y;
            }
        }

        if (Vector3.Distance(enemy.transform.position, destX) < 0.5f)
        {
           enemy.CurrentDest = (enemy.CurrentDest + 1) % enemy.dest.Length;
           Debug.Log("ChangeToJump");
            if (Random.Range(0, 2) == 0)
            { 
               enemy.ChangeState(new EnemyJump());
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



}
