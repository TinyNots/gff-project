using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData : MonoBehaviour
{
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        _shadow = transform.Find("Shadow");
        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.tag == "Attack")
        {
            float depthA = _shadow.GetComponent<Depth>().DepthSetting;
            float depthB = collision.GetComponent<Depth>().DepthSetting;

            if (depthB <= depthA + _shadowSize.y / 2.0f && depthB >= depthA - _shadowSize.y / 2.0f)
            {
                Debug.Log("Hit!!!");
            }
        }
    }
}
