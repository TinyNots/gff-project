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
    private bool _isHurt = false;

    [Header("Speed Setting")]
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _speedPercentZ = 0.5f;
    [SerializeField]
    private Animator _animator;

    private Vector2 _moveInput = new Vector2(0.0f, 0.0f);
    private Transform moveableArea;

    private void Start()
    {
        moveableArea = Camera.main.transform.Find("MoveableArea");
    }

    public void Update()
    {
        _depth = transform.position.y;
        if(_animator != null)
        {
            _animator.SetBool("Moveable", _isEableMove);
            _animator.SetBool("IsHurt", _isHurt);
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

        Collider2D moveableCollider = moveableArea.GetComponent<Collider2D>();
        Transform shadow = transform.Find("Shadow");
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(
            position.x,
            moveableArea.position.x - moveableCollider.bounds.size.x / 2.0f,
            moveableArea.position.x + moveableCollider.bounds.size.x / 2.0f);

        position.y = Mathf.Clamp(
             position.y,
             moveableArea.position.y - moveableCollider.bounds.size.y / 2.0f - (shadow.position.y - transform.position.y),
             moveableArea.position.y + moveableCollider.bounds.size.y / 2.0f - (shadow.position.y - transform.position.y));
        transform.position = position;
    }

    public float GetDepth()
    {
        return _depth;
    }

    public bool EnableMove
    {
        get { return _isEableMove; }
        set { _isEableMove = value; }
    }

    public bool EnableTurn
    {
        get { return _isEableTurn; }
        set { _isEableTurn = value; }
    }

    public bool IsHurt
    {
        get { return _isHurt; }
        set { _isHurt = value; }
    }

    public Vector2 MoveInput
    {
        get { return _moveInput; }
        set { _moveInput = value; }
    }
}