using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float _depth = 0.0f;
    private bool _isEableMove = true;
    private bool _isEableTurn = true;

    [Header("Speed Setting")]
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _speedPercentZ = 0.5f;
    [SerializeField]
    private Animator _animator;

    private Vector2 _moveInput = new Vector2(0.0f, 0.0f);

    public void Update()
    {
        _depth = transform.position.y;
        if(_animator != null)
        {
            _animator.SetBool("Moveable", _isEableMove);
            _animator.SetFloat("Speed", Mathf.Abs(_moveInput.x + _moveInput.y));
        }
    }

    private void FixedUpdate()
    {
        if(_isEableMove)
        {
            Vector2 movement = new Vector2(_moveInput.x, _moveInput.y * _speedPercentZ) * _speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    public float GetDepth()
    {
        return _depth;
    }

    public bool EableMove
    {
        get { return _isEableMove; }
        set { _isEableMove = value; }
    }

    public bool EableTurn
    {
        get { return _isEableTurn; }
        set { _isEableTurn = value; }
    }

    public Vector2 MoveInput
    {
        get { return _moveInput; }
        set { _moveInput = value; }
    }
}