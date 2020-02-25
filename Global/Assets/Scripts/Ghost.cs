using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField]
    private float _fadeTime;
    [SerializeField]
    private Sprite _sprite;

    [Header("Colors")]
    [SerializeField]
    private Color _trailColor;

    private SpriteRenderer _sr;
    private Color _originColor;

    void Start()
    {
        _sr = transform.GetComponent<SpriteRenderer>();

        _sr.color = _trailColor;
        _originColor = _sr.color;
    }

    // エフェクト残像の更新
    void Update()
    {
        if(_sr.color.a > 0)
        {
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, _sr.color.a - (_originColor.a / _fadeTime) * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
