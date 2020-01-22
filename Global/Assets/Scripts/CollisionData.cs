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
        if(collision.CompareTag("Player"))
        {
            Transform collisionShadow = collision.transform.parent.Find("Shadow");
            float collisionDepth = collisionShadow.GetComponent<Depth>().DepthSetting;
            Vector2 collisionShadowSize = collisionShadow.GetComponent<Renderer>().bounds.size;

            Rect shadowA = new Rect((Vector2)_shadow.position - _shadowSize / 2.0f, _shadowSize);
            Rect shadowB = new Rect((Vector2)collisionShadow.position - collisionShadowSize / 2.0f, collisionShadowSize);

            if(shadowA.Overlaps(shadowB,true))
            {
                Transform sprite = transform.Find("Sprite");
                sprite.GetComponent<Flasher>().StartFlash(0.2f);
                sprite.GetComponent<Animator>().SetBool("Collected", true);
                Destroy(gameObject, 0.3f);
            }
        }
    }
}
