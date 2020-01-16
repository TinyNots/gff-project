using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Jump Setting")]
    [SerializeField]
    private float _jumpVelocity = 10f;
    [SerializeField]
    private float _fallMultiplier = 3.0f;
    private float _currentVelocity = 0;
    private bool _isGrounded = true;
    private Character _character;
    [SerializeField]
    private Animator _animator;
    private ParticleSystem _particle;

    [Header("Others")]
    [SerializeField]
    private Vector2 _offset = new Vector2(0f, 0f);

    private Collider2D _shadowCollider;

    // Start is called before the first frame update
    void Start()
    {
        _character = gameObject.GetComponentInParent<Character>();
        _shadowCollider = transform.parent.Find("Shadow").GetComponent<Collider2D>();
        _offset = transform.localPosition;
        _particle = transform.parent.Find("Jump-Fall Particle").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && _character.EnableMove)
        {
            _isGrounded = false;
            _currentVelocity = _jumpVelocity;
        }

        if (!_isGrounded)
        {
            if (_currentVelocity <= 0)
            {
                _currentVelocity -= 10f * Time.deltaTime * _fallMultiplier;
            }
            else
            {
                _currentVelocity -= 10f * Time.deltaTime;
            }

            _animator.SetFloat("JumpVelocity", _currentVelocity);

            if (transform.localPosition.y < _offset.y - 0.01f)
            {
                transform.localPosition = _offset;
                _currentVelocity = 0;
                _isGrounded = true;
                _animator.SetBool("IsJumping", false);
                _shadowCollider.isTrigger = false;
                _particle.Play();
            }
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);
    }

    public void StartJump()
    {
        if(_isGrounded)
        {
            _isGrounded = false;
            _shadowCollider.isTrigger = true;
            _currentVelocity = _jumpVelocity;
            _particle.Play();
        }
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    public void AirLandAttack()
    {
        _character.EnableMove = false;
        _character.EnableTurn = false;
        _currentVelocity *= 2.0f;
    }

    public void ResetEableMove()
    {
        _character.EnableMove = true;
        _character.EnableTurn = true;
    }

    public void ResetHurt()
    {
        _character.IsHurt = false;
    }
}
