using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrder : MonoBehaviour
{
    private SpriteRenderer _sp;

    // Start is called before the first frame update
    void Start()
    {
        _sp = transform.Find("Sprite").transform.GetComponent<SpriteRenderer>();
        if(_sp == null)
        {
            Debug.LogError("Sprite is missing");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _sp.sortingOrder = (int)(transform.position.y * -100);
    }
}
