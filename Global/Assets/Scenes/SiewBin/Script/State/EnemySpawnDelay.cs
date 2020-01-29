using UnityEngine;

public class EnemySpawnDelay : IState<Enemy>
{
    float _waitTime = 1;
    float _timeStart;

    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        _timeStart = Time.time;
    }

    public void Execute(Enemy enemy)
    {
        if (Time.time > _timeStart + _waitTime)
        {
            var tmp = enemy.FindRandomPlayer();
            if (tmp.GetComponent<TargetNum>().TargettedNum < 5)
            {
                enemy._tmpPlayer = tmp;
                enemy._tmpPlayer.GetComponent<TargetNum>().TargettedNum++;

                enemy.IsTargeting = true;
                enemy.TargetChangeable = false;

            }

            enemy.ChangeState(new EnemyPatrol());
        }
    }

    public void Exit(Enemy enemy)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
