using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrder : MonoBehaviour
{
    private SpriteRenderer _sp;
    private Transform _shadow;

    // Start is called before the first frame update
    void Start()
    {
        _sp = transform.Find("Sprite").transform.GetComponent<SpriteRenderer>();
        if(_sp == null)
        {
            Debug.LogError("Sprite is missing");
            return;
        }

        _shadow = transform.Find("Shadow");
    }

    // Update is called once per frame
    void Update()
    {
        _sp.sortingOrder = (int)(_shadow.position.y * -100);
    }
}
