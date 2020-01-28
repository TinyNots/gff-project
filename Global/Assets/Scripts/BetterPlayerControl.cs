using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPlayerControl : MonoBehaviour
{
    private Controller _gamepad;
    private int _controllerIndex;

    [Header("Temporary")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _sprite;
    [SerializeField]
    private Jumper _jumpStatus;
    [SerializeField]
    private Character _character;
    [SerializeField]
    private float _stopTime = 0.05f;
    private ParticleSystem _dust;
    [SerializeField]
    private float _dustOverRate = 10.0f;
    private ParticleSystem.EmissionModule _dustEmission;
    [SerializeField]
    private Dasher _dasher;

    private Rigidbody2D _rb;

    // Debug
    [SerializeField]
    private Transform _tmpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //_controllerIndex = 0;
        _dust = transform.Find("Dust Particle").GetComponent<ParticleSystem>();
        _dustEmission = _dust.emission;

        _rb = transform.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.Log("Rigidbody is missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_controllerIndex == 0)
        {
            return;
        }

        _gamepad = GamePadManager.Instance.GetGamepad(_controllerIndex);

        // 移動関連
        _character.MoveInput = new Vector2(_gamepad.GetStickL().X, _gamepad.GetStickL().Y);
        if(_character.EnableTurn)
        {
            if (_gamepad.GetStickL().X < -0.01f)
            {
                _sprite.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (_gamepad.GetStickL().X > 0.01f)
            {
                _sprite.transform.localRotation = Quaternion.Euler(0f, 0, 0f);
            }
        }

        // ボタン関連
        if(_gamepad.GetButtonDown("Y") && _character.EnableAttack)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Last"))
            {
                return;
            }

            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Dash"))
            {
                _dasher.StopDash();
            }

            _animator.SetTrigger("Attack");
            if (_jumpStatus.GetIsGrounded())
            {
                _character.EnableMove = false;
            }
        }

        if(_gamepad.GetButtonDown("A"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack_Air3_End"))
            {
                return;
            }

            if(_dasher.IsDashing)
            {
                _animator.SetTrigger("JumpAttack");
                _dasher.StopDash();
            }

            _animator.SetBool("IsJumping", true);
            _jumpStatus.StartJump();
            _character.EnableAttack = true;
        }

        if(_gamepad.GetButtonDown("B"))
        {
            _dasher.StartDash();
            _character.EnableMove = false;
            _character.EnableTurn = false;
            _character.EnableAttack = true;
        }

        // Debug
        if(_gamepad.GetButtonDown("X"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Last"))
            {
                return;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Dash"))
            {
                _dasher.StopDash();
            }

            _animator.SetTrigger("Punch");
            if (_jumpStatus.GetIsGrounded())
            {
                _character.EnableMove = false;
            }
        }

        var health = _sprite.GetComponent<Health>();
        if (health.HP <= 0)
        {
            _character.IsDie = true;
        }

        if(_character.EnableMove && _jumpStatus.GetIsGrounded())
        {
            float movingSpeed = Mathf.Abs(_gamepad.GetStickL().X) + Mathf.Abs(_gamepad.GetStickL().Y);
            _dustEmission.rateOverTime = _dustOverRate * Mathf.Clamp01(movingSpeed);
        }
        else
        {
            _dustEmission.rateOverTime = 0.0f;
        }
    }

    public void SetControllerIndex(int index)
    {
        _controllerIndex = index;
    }

    public void RumbleController(float timer, float fadeTime,Vector2 power)
    {
        _gamepad.AddRumble(timer, fadeTime, power);
    }

    public Controller GetGamepad()
    {
        return _gamepad;
    }
}
