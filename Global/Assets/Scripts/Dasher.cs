using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform _sprite;

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

    [Header("Clone Setting")]
    [SerializeField]
    private Transform _clonePrefab;
    [SerializeField]
    private int _maxClone = 4;

    private List<Transform> _clones;
    [SerializeField]
    private SkillManager _skillManager;
    private Jumper _jumpState;

    [SerializeField]
    private Transform _shadow;

    private void Start()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.Log("Rigidbody is missing");
        }

        _sprite = transform.Find("Sprite");
        if(_sprite == null)
        {
            Debug.LogError("Sprite is missing");
        }

        _animator = _sprite.GetComponent<Animator>();
        _character = transform.GetComponent<Character>();
        _dashParticle = transform.Find("DashParticle").GetComponent<ParticleSystem>();

        _gamepad = GetComponent<BetterPlayerControl>().GetGamepad();
        _stickVelocity = Vector2.zero;
        _isDashing = false;

        // clone init
        _clones = null;
        _jumpState = _sprite.GetComponent<Jumper>();
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
            Transform ghost = Instantiate(_ghostPrefab, _sprite.position, _sprite.rotation);
            ghost.GetComponent<SpriteRenderer>().sprite = _sprite.GetComponent<SpriteRenderer>().sprite;

            _rb.velocity = _stickVelocity * _velocity;

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
            SoundManager.Instance.PlaySe("Dash Light Armor 2_04");

            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(_sprite.position));

            _timer = _dashTime;
            _dashParticle.Play();

            if (_gamepad == null)
            {
                _gamepad = GetComponent<BetterPlayerControl>().GetGamepad();
            }

            if (_sprite.transform.eulerAngles.y == 180.0f)
            {
                _stickVelocity = new Vector2(-1.0f, _gamepad.GetStickL().Y / 2.0f);
            }
            else
            {
                _stickVelocity = new Vector2(1.0f, _gamepad.GetStickL().Y / 2.0f);
            }
            _isDashing = true;

            if(_jumpState.GetIsGrounded())
            {
                SpawnClone();
            }
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

    private void SpawnClone()
    {
        if(_clones == null)
        {
           _clones = _skillManager.GetClones();
        }

        if(_clones.Count < _maxClone)
        {
            if(!_skillManager.GetTrigger())
            {
                Transform clone = Instantiate(_clonePrefab, _shadow.position, Quaternion.identity);
                clone.Find("Sprite").rotation = _sprite.rotation;
                _clones.Add(clone);

            }
        }
    }
}
