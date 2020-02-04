using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float _speed = 150.0f;
    [SerializeField]
    private Collider2D _collider;

    private Transform _sprite;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = transform.Find("Sprite");
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        transform.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * _speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(_sprite.localPosition.y <= 0)
        {
            _rb.velocity = Vector2.zero;
            _collider.enabled = true;
        }
    }
}
