using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//現時点は敵のみ遠攻撃ができる
public class Projectile : MonoBehaviour
{
    float _depth;
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);
    private float multiShotTimer;
    private int multiShotIdx;
    private bool multiShotFlag = false;
    private bool sigmoidMove = false;
    private bool reverseSigmoid = false;
    float theta = 0;

    public bool SigmoidMove
    {
        set { sigmoidMove = value; }
    }

    public bool ReverseSigmoid
    {
        set { reverseSigmoid = value; }
    }

  

    // Start is called before the first frame update
    void Start()
    {
        _shadow = transform.Find("Shadow");
        if(_shadow == null)
        {
            _shadow = transform.Find("ShadowRotation").Find("Shadow");
        }
       // multiShotIdx = 0;
        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
        multiShotTimer = Time.time;

    }
    // Update is called once per frame
    void Update()
    {

        if (gameObject.activeSelf)
        {
            if (!multiShotFlag)
            {
                if (sigmoidMove)
                {
                    if (reverseSigmoid)
                    {
                        gameObject.transform.position += gameObject.transform.TransformDirection(0.1f, -gameObject.transform.position.x / (1f + Mathf.Abs(gameObject.transform.position.x)) * Time.deltaTime, 0.0f);
                    }
                    else
                    {
                        gameObject.transform.position += gameObject.transform.TransformDirection(0.1f, gameObject.transform.position.x / (1f + Mathf.Abs(gameObject.transform.position.x)) * Time.deltaTime, 0.0f);
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
        //スクリーン外だったら廃棄する
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (gameObject.transform.position.x > wsize.x || gameObject.transform.position.x < -wsize.x ||
            gameObject.transform.position.y > wsize.y|| gameObject.transform.position.y < -wsize.y)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    _depth = _shadow.GetComponent<Depth>().DepthSetting;

        //    float depthB = collision.gameObject.transform.parent.Find("Shadow").GetComponent<Depth>().DepthSetting;
        //    if (depthB <= _depth + _shadowSize.y / 2.0f && depthB >= _depth - _shadowSize.y / 2.0f)
        //    {
        //        if (collision.gameObject.GetComponent<Health>().HP > 0)
        //        {
        //            Destroy(gameObject);
        //        }
        //    }
        //}
    }

    public void NormalShot()
    {
        gameObject.transform.position += gameObject.transform.TransformDirection(0.1f, 0.0f, 0.0f);
    }

    public void MultiShot()
    {
        switch(multiShotIdx)
        {
            case 1:
                if (Time.time < multiShotTimer + 1)
                {
                    gameObject.transform.position += gameObject.transform.TransformDirection(0.0f, 2.0f, 0.0f) * Time.deltaTime;
                }
                break;
            case 3:
                if (Time.time < multiShotTimer + 1)
                {
                    gameObject.transform.position += gameObject.transform.TransformDirection(0.0f, -2.0f, 0.0f) * Time.deltaTime;
                }
                break;

        }
        gameObject.transform.position += gameObject.transform.TransformDirection(0.1f, 0.0f, 0.0f);

    }



    public int MultiShotIdx
    {
        set { multiShotIdx = value;
            multiShotFlag = true;
        }
        get { return multiShotIdx; }
    }

    public void SpiralShot()
    {
        float x =  Mathf.Cos(theta) * theta *0.5f;
        float z = transform.position.z;
        float y = Mathf.Sin(theta)* theta * 0.5f;

        transform.position +=  new Vector3(x, y, 0) * Time.deltaTime;
        theta += 0.1f;
        theta %= 360;
       // transform.position += gameObject.transform.TransformDirection(0.1f, 0.0f, 0.0f);
    }
}
