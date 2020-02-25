using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveMng : MonoBehaviour
{
    private float _waitTime =0; //敵を検査するまでの必要時間
    private float _timer = 0;   //生成したから経った時間
    private int _waveCnt =0;    //現在のウェイブ数
    private bool _waveReadyFlag = true;
    public Wave[] _waveSpawn;   //ウェイブ情報
    public EnemyFactory[] _factoryCnt = new EnemyFactory[3];
    public Score _score;        //ランキングに記録する
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
        ////デバッグ機能
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    var go = GameObject.FindGameObjectsWithTag("Enemy");
        //    foreach (GameObject gos in go)
        //    {
        //        Destroy(gos.transform.parent.gameObject);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    _waveReadyFlag = true;
        //    _animator.SetTrigger("Change");
        //}
        ////--
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
        int tmpWaveCnt = _waveCnt;
        if (tmpWaveCnt >= _waveSpawn.Length)
        {
            tmpWaveCnt = _waveCnt % _waveSpawn.Length;
        }
        if (_waveReadyFlag)
        {
            Debug.Log("Wave" + (_waveCnt + 1));
            _waitTime = 0;
            //複数のパターンに対応
            for (int i = 0; i < _waveSpawn[tmpWaveCnt].wave.Length; i++)
            {
                _factoryCnt[i].WaveInit(_waveSpawn[tmpWaveCnt].wave[i]);
                _factoryCnt[i].gameObject.SetActive(true);
                var tmpInfo = _waveSpawn[tmpWaveCnt].wave[i];
                var tmpWaitTime = tmpInfo._spawnTime * tmpInfo._maxCnt / tmpInfo._massSpawnCnt + 3;
                if (tmpWaitTime > _waitTime)
                {
                    _waitTime = tmpWaitTime;
                }
                //難易度を増加
                if (_waveCnt >= _waveSpawn.Length)
                {
                    _waveSpawn[tmpWaveCnt].wave[i]._maxCnt *= 2;
                }
            }
            _waveCnt ++;
            _score.AddScore(1);
            _waveReadyFlag = false;
        }
    }

    public bool GetFlag()
    {
        return _waveReadyFlag;
    }
}

[System.Serializable]
public class Wave
{
    public FactoryInfo[] wave;
}