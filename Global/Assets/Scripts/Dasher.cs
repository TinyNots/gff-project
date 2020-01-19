using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField]
    private float _velocity = 20.0f;
    [SerializeField]
    private float _timer = 0.0f;
    [SerializeField]
    private float _dashTime = 0.2f;

    private Animator _animator;
    private Character _character;


    private void Start()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.Log("Rigidbody is missing");
        }

        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _character = transform.GetComponent<Character>();
    }

    private void Update()
    {
        _animator.SetFloat("DashVelocity", _timer);

       if(_timer == 0.0f)
        {
            return;
        }

        if(_timer > 0)
        {
            if (transform.Find("Sprite").transform.eulerAngles.y == 180.0f)
            {
                _rb.velocity = Vector2.left * _velocity;
            }
            else
            {
                _rb.velocity = Vector2.right * _velocity;
            }

            _timer -= Time.deltaTime;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _character.EnableMove = true;
            _character.EnableTurn = true;
            _timer = 0;
        }
    }

    public void StartDash()
    {
        _timer = _dashTime;
    }
}
