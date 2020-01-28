using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveMng : MonoBehaviour
{
    private float _waitTime =0;
    private float _timer = 0;
    private int _waveCnt = 2;
    private bool _waveReadyFlag = true;
    public Wave[] _waveSpawn;
    public EnemyFactory[] _factoryCnt = new EnemyFactory[3];
    public Score _score;
    private GameObject _enemy;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <_factoryCnt.Length; i++)
        {
            _factoryCnt[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaveStart();
        _timer += Time.deltaTime;
        if (_timer > _waitTime)
        {
            _enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (_enemy == null)
            {
                _timer = 0;
                _waveReadyFlag = true;
            }
            else
            {
                _waitTime += 5;
            }
        }
       
    }

    void WaveStart()
    {
        int tmpCnt = _waveCnt;
        if (tmpCnt >= _waveSpawn.Length)
        {
            tmpCnt = _waveSpawn.Length - 1;
        }
        if (_waveReadyFlag)
        {
            Debug.Log("Wave" + (_waveCnt + 1));
            _waitTime = 0;
            for (int i = 0; i < _waveSpawn[tmpCnt].wave.Length; i++)
            {
                _factoryCnt[i].WaveInit(_waveSpawn[tmpCnt].wave[i]);
                _factoryCnt[i].gameObject.SetActive(true);
                var tmpInfo = _waveSpawn[tmpCnt].wave[i];
                var tmpWaitTime = tmpInfo._spawnTime * tmpInfo._maxCnt / tmpInfo._massSpawnCnt + 3;
                if (tmpWaitTime > _waitTime)
                {
                    _waitTime = tmpWaitTime;
                }
                if (_waveCnt >= _waveSpawn.Length)
                {
                    _waveSpawn[tmpCnt].wave[i]._maxCnt += 1;
                }
            }
            if (_waitTime < 8)
            {
                _waitTime = 8;
            }
            _waveCnt ++;
            _score.AddScore(1);
            _waveReadyFlag = false;
        }
    }
}

[System.Serializable]
public class Wave
{
    public FactoryInfo[] wave;
}

