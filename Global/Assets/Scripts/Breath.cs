using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    private SpriteRenderer _sr;

    [Header("General")]
    [SerializeField]
    private float _minAlpha = 0.175f;
    [SerializeField]
    private float _maxAlpha = 0.5f;

    [Header("Internval")]
    [SerializeField]
    private float _value = 0.01f;
    private float _counter;

    // Start is called before the first frame update
    void Start()
    {
        _sr = transform.GetComponent<SpriteRenderer>();
        if(_sr == null)
        {
            Debug.LogError("Sprite is missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = _minAlpha + (_maxAlpha - _minAlpha) * Mathf.Sin(_counter) * Mathf.Sin(_counter);
        _sr.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _counter += _value;
    }
}
