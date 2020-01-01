﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    Enemy prototype;
    public enum SpawnType
    {
        Side,
        OneTime,
        Top
    }
    [SerializeField]
    bool fromRight = true;
    bool isSpawned = false;
    [SerializeField]
    SpawnType spawnType;
    int curCnt = 0;
    int maxCnt = 10;
    float timer = 0;
    float spawnTime = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (curCnt < maxCnt)
        {
            if (timer >= spawnTime)
            {

                switch (spawnType)
                {
                    case SpawnType.Side:
                        SideSpawn(fromRight);
                        break;
                    case SpawnType.OneTime:
                        OneTimeSpawn(fromRight);
                        break;
                    case SpawnType.Top:
                        TopSpawn();
                        break;
                    default:
                        Debug.Log("Spawn Null");
                        break;

                }
                curCnt++;
                timer = 0;
                if (Random.Range(0, 2) == 1)
                {
                    fromRight = !fromRight;
                }
            }
            timer += Time.deltaTime;
            
        }
        
    }

    Enemy Spawn()
    {
        return Instantiate(prototype, this.transform.position, Quaternion.identity);
    }

    void SideSpawn(bool fromRight = true)
    {
        var enemy = Spawn();
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        if (fromRight)
        {
            enemy.transform.position = new Vector3(wsize.x + 1, Random.Range(0 - wsize.y / 2, 2.8f), 0);
        }
        else
        {
            enemy.transform.position = new Vector3( -wsize.x - 1, Random.Range(0 - wsize.y / 2, 2.8f), 0);

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
            enemy.transform.position = new Vector3( wsize.x + 1 , Random.Range(0 - wsize.y / 2,2.8f), 0);
        }
        else
        {
            enemy.transform.position = new Vector3( -wsize.x - 1, Random.Range(0 - wsize.y / 2, 2.8f), 0);

        }
        isSpawned = true;
        Debug.Log("One Time Spawn");

    }
    void TopSpawn(bool fromRight = false)
    {
        //var enemy = Spawn();
        //var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        //enemy.transform.position = new Vector3(Random.Range(0 - wsize.x / 2, 0 + wsize.x), Random.Range(0 - wsize.y / 2, 2.8f), 0);
        //enemy.IsJumping = true;
        //var currentVelocity = 0f;
        //while (currentVelocity < -10f)
        //{ 
        //    currentVelocity -= 10f * Time.deltaTime;
        //    enemy.transform.position += enemy.transform.TransformDirection(0, 0.5f * currentVelocity * Time.deltaTime, 0.0f);

        //}
        //Debug.Log("Top Spawn");

    }
}
