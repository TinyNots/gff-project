using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveMng : MonoBehaviour
{
    private float waitTime =0;
    private float timer = 0;
    private int waveCnt = 0;
    private bool waveReadyFlag = true;
    public Wave[] waveSpawn;
    public EnemyFactory[] factoryCnt = new EnemyFactory[3];
    public Score score;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <factoryCnt.Length; i++)
        {
            factoryCnt[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaveStart();
        timer += Time.deltaTime;
        if ( timer >waitTime)
        {
            timer = 0;
            waveReadyFlag = true;
        }
       
    }

    void WaveStart()
    {
        int tmpCnt = waveCnt;
        if (tmpCnt >= waveSpawn.Length)
        {
            tmpCnt = waveSpawn.Length - 1;
        }
        if (waveReadyFlag)
        {
            Debug.Log("Wave" + (waveCnt + 1));
            waitTime = 0;
            for (int i = 0; i < waveSpawn[tmpCnt].wave.Length; i++)
            {
                factoryCnt[i].WaveInit(waveSpawn[tmpCnt].wave[i]);
                factoryCnt[i].gameObject.SetActive(true);
                var tmpInfo = waveSpawn[tmpCnt].wave[i];
                var tmpWaitTime = tmpInfo.spawnTime * tmpInfo.maxCnt / tmpInfo.massSpawnCnt;
                if (tmpWaitTime > waitTime)
                {
                    waitTime = tmpWaitTime;
                }
                if (waveCnt >= waveSpawn.Length)
                {
                    waveSpawn[tmpCnt].wave[i].maxCnt += 1;
                }
            }
            if (waitTime < 8)
            {
                waitTime = 8;
            }
            waveCnt ++;
            score.AddScore(1);
            waveReadyFlag = false;
        }
    }
}

[System.Serializable]
public class Wave
{
    public FactoryInfo[] wave;
}

