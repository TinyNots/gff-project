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
    [SerializeField]
    private Transform _ghostPrefab;
    [SerializeField]
    private int _ghostCount = 2;

    private Animator _animator;
    private Character _character;
    private ParticleSystem _dashParticle;

    private Controller _gamepad;
    private Vector2 _stickVelocity;

    private bool _isDashing;

    private void Start()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.Log("Rigidbody is missing");
        }

        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _character = transform.GetComponent<Character>();
        _dashParticle = transform.Find("DashParticle").GetComponent<ParticleSystem>();

        _gamepad = GetComponent<BetterPlayerControl>().GetGamepad();
        _stickVelocity = Vector2.zero;
        _isDashing = false;
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
            Transform ghost = Instantiate(_ghostPrefab, transform.Find("Sprite").position, transform.Find("Sprite").rotation);
            ghost.GetComponent<SpriteRenderer>().sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;

            if (_gamepad == null)
            {
                _gamepad = GetComponent<BetterPlayerControl>().GetGamepad();
            }

            _rb.velocity = _stickVelocity * _velocity;

            //if (transform.Find("Sprite").transform.eulerAngles.y == 180.0f)
            //{
            //    _rb.velocity = Vector2.left * _velocity;
            //}
            //else
            //{
            //    _rb.velocity = Vector2.right * _velocity;
            //}

            _timer -= Time.deltaTime;
        }
        else
        {
            StopDash();
        }
    }

    public void StartDash()
    {
        if(_timer == 0)
        {
            _timer = _dashTime;
            _dashParticle.Play();
          
            if (transform.Find("Sprite").transform.eulerAngles.y == 180.0f)
            {
                _stickVelocity = new Vector2(-1.0f, _gamepad.GetStickL().Y / 2.0f);
            }
            else
            {
                _stickVelocity = new Vector2(1.0f, _gamepad.GetStickL().Y / 2.0f);
            }
            _isDashing = true;
        }
    }

    public void StopDash()
    {
        _rb.velocity = Vector2.zero;
        _character.EnableMove = true;
        _character.EnableTurn = true;
        _timer = 0;
        _dashParticle.Stop();
        _isDashing = false;
    }

    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }
}
