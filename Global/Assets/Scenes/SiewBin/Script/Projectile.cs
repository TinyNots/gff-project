using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//現時点は敵のみ遠攻撃ができる
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            gameObject.transform.position += gameObject.transform.TransformDirection(0.1f, 0.0f, 0.0f);
        }
        //スクリーン外だったら廃棄する
        var wsize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (gameObject.transform.position.x > wsize.x || gameObject.transform.position.x < -wsize.x)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
