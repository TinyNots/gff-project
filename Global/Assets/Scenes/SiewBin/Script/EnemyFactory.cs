using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FactoryInfo
{
    public Enemy _prototype;
    public enum SpawnType
    {
        Side,   //左右から生成
        OneTime,//一度だけ生成
        Top     //上から生成
    }
    public bool _fromRight; //右から生成
    public SpawnType _spawnType;    //生成する方法
    public int _maxCnt;        //最大生成数
    public float _spawnTime;    //生成要る時間
    public int _massSpawnCnt;  //大量生成数
}

public class EnemyFactory : MonoBehaviour
{
    FactoryInfo initInfo;
    //[SerializeField]
    //Enemy prototype;
    //public enum SpawnType
    //{
    //    Side,   //左右から生成
    //    OneTime,//一度だけ生成
    //    Top     //上から生成
    //}
    //[SerializeField]
    //bool fromRight = true; //右から生成
    //[SerializeField]
    //SpawnType spawnType;    //生成する方法
    //public int maxCnt = 10;        //最大生成数
    //public float spawnTime = 3;    //生成要る時間
    //public int massSpawnCnt = 1;  //大量生成数
    private float timer = 0;        //次の生成までの時間カウンター
    private int curCnt = 0;         //現在の敵数

    private bool isSpawned = false; //生成したか


    // Start is called before the first frame update
    void Start()
    {
        //initInfo._fromRight = true;
        //initInfo._maxCnt = 10;
        //initInfo._spawnTime = 3;
        //initInfo._massSpawnCnt = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (curCnt < initInfo._maxCnt)
        {
            if (timer >= initInfo._spawnTime)
            {
                for (int i = 0; i < initInfo._massSpawnCnt; i++)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        initInfo._fromRight = !initInfo._fromRight;
                    }
                    switch (initInfo._spawnType)
                    {
                        case FactoryInfo.SpawnType.Side:
                            SideSpawn(initInfo._fromRight);
                            break;
                        case FactoryInfo.SpawnType.OneTime:
                            OneTimeSpawn(initInfo._fromRight);
                            break;
                        case FactoryInfo.SpawnType.Top:
                            TopSpawn();
                            break;
                        default:
                            Debug.Log("Spawn Null");
                            break;

                    }
                    curCnt++;
                }
                if (initInfo._spawnType == FactoryInfo.SpawnType.OneTime)
                {
                    isSpawned = true;
                }
                timer = 0;
              
            }
            timer += Time.deltaTime;
            
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    Enemy Spawn()
    {
        return Instantiate(initInfo._prototype, this.transform.position, Quaternion.identity);
    }

    void SideSpawn(bool fromRight = true)
    {
        var enemy = Spawn();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        if (fromRight)
        {
            enemy.transform.position = new Vector3(wsize.x , Random.Range(0 - wsize.y / 2, 2.8f), 0);
            enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else
        {
            enemy.transform.position = new Vector3( -wsize.x , Random.Range(0 - wsize.y / 2, 2.8f), 0);

        }
        if (initInfo._prototype.IsBoss)
        {
            enemy.transform.position = new Vector3(-wsize.x, 0, 0);

        }
        Debug.Log("Side Spawn");
    }

    void OneTimeSpawn(bool fromRight = true)
    {
        if (isSpawned) return;
        var enemy = Spawn();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        if (fromRight)
        {
            enemy.transform.position = new Vector3( wsize.x +  2, Random.Range(0 - wsize.y / 2,2.8f), 0);
            enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else
        {
            enemy.transform.position = new Vector3( -wsize.x -  2, Random.Range(0 - wsize.y / 2, 2.8f), 0);

        }
        initInfo._maxCnt = 1;
        Debug.Log("One Time Spawn");

    }
    void TopSpawn(bool fromRight = false)
    {
        var enemy = Spawn();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        enemy.transform.position = new Vector3(Random.Range(0 - wsize.x / 2 - 2, 0 + wsize.x + 2), Random.Range(0 - wsize.y / 2, 2.8f), 0);
        if(Random.Range(0,2) == 1)
        {
            enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        }
        enemy.ShadowOffSet = initInfo._prototype.transform.Find("Sprite").transform.position.y - initInfo._prototype.transform.Find("Shadow").transform.position.y;

        enemy.transform.Find("Sprite").transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + wsize.y, 0);
        //enemy.transform.position = new Vector3(enemy.ShadowPos.x, enemy.ShadowPos.y + wsize.y, enemy.ShadowPos.z);
        enemy.IsJumping = true;
       
        Debug.Log("Top Spawn");

    }

    public void WaveInit(FactoryInfo info)
    {
        initInfo = info;
        curCnt = 0;         //現在の敵数

        isSpawned = false; //生成したか
    }
   
}
