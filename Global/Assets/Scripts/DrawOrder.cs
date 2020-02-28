using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sp;
    [SerializeField]
    private Transform _shadow;

    void Start()
    {
        if(_sp == null)
        {
            _sp = transform.Find("Sprite").transform.GetComponent<SpriteRenderer>();
            if (_sp == null)
            {
                Debug.LogError("Sprite is missing");
                return;
            }
        }

        if (_shadow == null)
        {
            _shadow = transform.Find("Shadow");
        }
    }

    // 描画順序はキャラの影に基づいて順序する
    void Update()
    {
        _sp.sortingOrder = (int)(_shadow.position.y * -100);
    }
}
