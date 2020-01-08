using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMng : MonoBehaviour
{ 
    public int waveCnt = 30;
    public int factoryCnt = 3;
    public EnemyFactory[,] waveSpawn;
    // Start is called before the first frame update
    void Start()
    {
        waveSpawn = new EnemyFactory[waveCnt,factoryCnt];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
