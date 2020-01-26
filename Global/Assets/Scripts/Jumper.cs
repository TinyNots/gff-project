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

    private float _forwardSpeed;

    private Collider2D _shadowCollider;

    // Start is called before the first frame update
    void Start()
    {
        _character = gameObject.GetComponentInParent<Character>();
        _shadowCollider = transform.parent.Find("Shadow").GetComponent<Collider2D>();
        _offset = transform.localPosition;
        _forwardSpeed = 0.0f;

        if(transform.parent.Find("Jump-Fall Particle") != null)
        {
            _particle = transform.parent.Find("Jump-Fall Particle").GetComponent<ParticleSystem>();
        }

    }

    // Update is called once per frame
    void Update()
    {
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
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Kick_Air_Loop"))
                {
                    _character.EnableMove = true;
                    _character.EnableTurn = true;
                }

                transform.localPosition = _offset;
                _currentVelocity = 0;
                _isGrounded = true;
                _animator.SetBool("IsJumping", false);
                //_shadowCollider.isTrigger = false;
                _particle.Play();
               
                if (_forwardSpeed != 0.0f)
                {
                    _forwardSpeed = 0.0f;
                    transform.GetComponent<AnimationEvents>().DelayAttack();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);

        if(_forwardSpeed != 0.0f)
        {
            if(transform.eulerAngles.y == 180.0f)
            {
                _character.transform.Translate(Vector2.left * _forwardSpeed * Time.deltaTime);
            }
            else
            {
                _character.transform.Translate(Vector2.right * _forwardSpeed * Time.deltaTime);
            }
        }
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
        _currentVelocity = -20.0f;
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

    public void DropKick(float speed)
    {
        _currentVelocity = -20.0f;
        _character.EnableMove = false;
        _character.EnableTurn = false;
        _forwardSpeed = speed;
        SoundManager.Instance.PlaySe("Whoosh 4_" + Random.Range(1, 5));
    }
}
