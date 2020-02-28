using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float _depth = 0.0f;
    private bool _isEnableMove = true;
    private bool _isEnableTurn = true;
    private bool _isEnableAttck = true;
    private bool _isHurt = false;
    private bool _isDie = false;

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
        moveableArea = GameObject.Find("MoveableArea").transform;
    }

    public void Update()
    {
        _depth = transform.position.y;
        if(_animator != null)
        {
            _animator.SetBool("Moveable", _isEnableMove);
            _animator.SetBool("IsHurt", _isHurt);
            _animator.SetFloat("Speed", Mathf.Abs(_moveInput.x + _moveInput.y));
            _animator.SetBool("IsDie", _isDie);
            _animator.SetBool("EnableAttack", _isEnableAttck);
        }
    }

    private void FixedUpdate()
    {
        if(_isEnableMove)
        {
            // 移動制御
            Vector2 movement = new Vector2(_moveInput.x, _moveInput.y * _speedPercentZ) * _speed * Time.deltaTime;
            transform.Translate(movement);
        }

        // 画面外に出させない
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

    // 奥行きの値をゲットする
    public float GetDepth()
    {
        return _depth;
    }

    // ゲットセット関連
    public bool EnableMove
    {
        get { return _isEnableMove; }
        set { _isEnableMove = value; }
    }

    public bool EnableTurn
    {
        get { return _isEnableTurn; }
        set { _isEnableTurn = value; }
    }

    public bool EnableAttack
    {
        get { return _isEnableAttck; }
        set { _isEnableAttck = value; }
    }

    public bool IsHurt
    {
        get { return _isHurt; }
        set
        {
            _isHurt = value;
            if(_isHurt)
            {
                _isEnableMove = false;
                _isEnableTurn = false;
            }
        }
    }

    public bool IsDie
    {
        get { return _isDie; }
        set
        {
            _isDie = value;
            if(_isDie)
            {
                _isEnableMove = false;
                _isEnableTurn = false;
            }
        }
    }

    public Vector2 MoveInput
    {
        get { return _moveInput; }
        set { _moveInput = value; }
    }
}