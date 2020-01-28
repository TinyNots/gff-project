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
            enemy._tmpPlayer = enemy.FindRandomPlayer();
            enemy.TargetChangeable = false;
            enemy.IsTargeting = true;


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
