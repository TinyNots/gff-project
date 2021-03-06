﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//現時点は敵のみ遠攻撃ができる
public class Projectile : MonoBehaviour
{
    private Transform _shadow;  //影
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);
    private float _multiShotTimer;
    private int _multiShotIdx;
    private bool _multiShotFlag = false;
    private bool _sigmoidMove = false;
    private bool _reverseSigmoid = false;
    float _theta = 0;

    [SerializeField]
    private float _speed = 0.1f;

    public bool SigmoidMove
    {
        set { _sigmoidMove = value; }
    }

    public bool ReverseSigmoid
    {
        set { _reverseSigmoid = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _shadow = transform.Find("Shadow");
        if(_shadow == null)
        {
            _shadow = transform.Find("ShadowRotation").Find("Shadow");
        }
        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
        _multiShotTimer = Time.time;

    }
    // Update is called once per frame
    void Update()
    {
        //スクリーン外だったら廃棄する
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (gameObject.transform.position.x > wsize.x  + 2f || gameObject.transform.position.x < -wsize.x -2f||
            gameObject.transform.position.y > wsize.y|| gameObject.transform.position.y < -wsize.y)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            if (!_multiShotFlag)
            {
                //シグモイド関数で弾を曲げる
                if (_sigmoidMove)
                {
                    if (_reverseSigmoid)
                    {
                        gameObject.transform.position += gameObject.transform.TransformDirection(_speed, -gameObject.transform.position.x / (1f + Mathf.Abs(gameObject.transform.position.x)) * Time.deltaTime, 0.0f);
                    }
                    else
                    {
                        gameObject.transform.position += gameObject.transform.TransformDirection(_speed, gameObject.transform.position.x / (1f + Mathf.Abs(gameObject.transform.position.x)) * Time.deltaTime, 0.0f);
                    }
                }
                else
                {
                    NormalShot();
                }
            }
            else
            {
                MultiShot();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void NormalShot()
    {
        gameObject.transform.position += gameObject.transform.TransformDirection(_speed, 0.0f, 0.0f);
    }

    public void MultiShot()
    {
        switch(_multiShotIdx)
        {
            case 1:
                if (Time.time < _multiShotTimer + 1)
                {
                    gameObject.transform.position += gameObject.transform.TransformDirection(0.0f, 2.0f, 0.0f) * Time.deltaTime;
                }
                break;
            case 3:
                if (Time.time < _multiShotTimer + 1)
                {
                    gameObject.transform.position += gameObject.transform.TransformDirection(0.0f, -2.0f, 0.0f) * Time.deltaTime;
                }
                break;

        }
        gameObject.transform.position += gameObject.transform.TransformDirection(_speed, 0.0f, 0.0f);

    }



    public int MultiShotIdx
    {
        set { _multiShotIdx = value;
            _multiShotFlag = true;
        }
        get { return _multiShotIdx; }
    }

    public void SpiralShot()
    {
        float x =  Mathf.Cos(_theta) * _theta *0.5f;
        float z = transform.position.z;
        float y = Mathf.Sin(_theta)* _theta * 0.5f;

        transform.position +=  new Vector3(x, y, 0) * Time.deltaTime;
        _theta += 0.1f;
        _theta %= 360;
       // transform.position += gameObject.transform.TransformDirection(0.1f, 0.0f, 0.0f);
    }
}
