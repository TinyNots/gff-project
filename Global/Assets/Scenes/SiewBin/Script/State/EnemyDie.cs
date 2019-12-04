using UnityEngine;

public class EnemyDie : IState<Enemy>
{
    float dieTime = 0;
    public void Enter(Enemy enemy)
    {
        enemy.GetComponent<Animator>().SetTrigger("Dying");
        enemy.GetComponent<BoxCollider2D>().enabled = false;
        dieTime = Time.time;
    }

    public void Execute(Enemy enemy)
    {
        var anim = enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        if (Time.time > dieTime + anim.length)
        {
            Debug.Log("Destroy enemy");
            enemy.DestroySelf();
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
