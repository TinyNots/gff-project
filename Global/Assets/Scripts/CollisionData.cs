using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData : MonoBehaviour
{
    [SerializeField]
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        _shadowSize = transform.Find("Shadow").transform.GetComponent<Renderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.tag == "Attack")
        {
            float depthA = transform.GetComponent<Character>().GetDepth() - 0.5f;
            float depthB = collision.GetComponent<Depth>().DepthSetting + 0.5f;

            if (depthB <= depthA + _shadowSize.y / 2.0f && depthB >= depthA - _shadowSize.y / 2.0f)
            {
                Debug.Log("Hit!!!");
            }

            //var cSize = collision.GetComponent<BoxCollider2D>().size;
            //var cPos = collision.transform.position;

            //var cShadow = cPos.y + cSize.y;
            //var offset = 2.5f;

            //if (cShadow <= transform.position.y + offset && cShadow >= transform.position.y - offset)
            //{
            //    Debug.Log("Hit!!!");
            //}
        }
    }
}
