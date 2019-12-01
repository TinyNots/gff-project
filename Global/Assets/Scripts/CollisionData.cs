using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            var cSize = collision.GetComponent<BoxCollider2D>().size;
            var cPos = collision.transform.position;

            var cShadow = cPos.y + cSize.y;
            var offset = 2.5f;

            if (cShadow <= transform.position.y + offset && cShadow >= transform.position.y - offset)
            {
                Debug.Log("Hit!!!");
            }
        }
    }
}
