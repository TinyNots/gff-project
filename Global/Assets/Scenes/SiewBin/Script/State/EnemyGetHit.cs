using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float hitTime;
    private float hitLen;
    private float tiltLen;
    public void Enter(Enemy enemy)
    {
        enemy.GetComponent<Animator>().SetTrigger("Hit");
        hitTime = Time.time;
        hitLen = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
        tiltLen = 2;
    }

    public void Execute(Enemy enemy)
    {
        switch (tiltLen % 2)
        {
            case 0:
                enemy.transform.position += enemy.transform.TransformDirection(0.1f,0f,0f);
                tiltLen--;
                break;
            case 1:
                enemy.transform.position -= enemy.transform.TransformDirection(0.1f, 0f,0f);
                tiltLen--;

                break;
        }
        if (Time.time > hitTime + hitLen)
        {
            enemy.ChangeState(new EnemyPatrol());
            return;
        }
       
    }
    public void Exit(Enemy enemy)
    {

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
