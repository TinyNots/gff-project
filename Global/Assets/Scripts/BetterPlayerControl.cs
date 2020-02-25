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
    [SerializeField]
    private SkillManager _skillManager;

    private Health _heaith;
    private Resurrection _resurrection;
    public Resurrection Resurrect
    {
        get { return _resurrection; }
        set { _resurrection = value; }
    }
    private int pushCount;

    private Rigidbody2D _rb;

    // 初期化
    void Start()
    {
        _dust = transform.Find("Dust Particle").GetComponent<ParticleSystem>();
        _dustEmission = _dust.emission;

        _rb = transform.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("Rigidbody is missing");
        }
        pushCount = 0;
    }

    // 更新
    void Update()
    {
        if (_controllerIndex == 0)
        {
            return;
        }

        _gamepad = GamePadManager.Instance.GetGamepad(_controllerIndex);
        if (_gamepad == null || !_gamepad.IsConnected)
        {
            return;
        }

        // 入力の値を渡す
        _character.MoveInput = new Vector2(_gamepad.GetStickL().X, _gamepad.GetStickL().Y);
        if (_character.EnableTurn)
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

        // 強攻撃
        if (_gamepad.GetButtonDown("Y") && _character.EnableAttack)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Last"))
            {
                return;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Dash"))
            {
                _dasher.StopDash();
            }

            if (_heaith.HP < 0)
            {
                if (_heaith.transform.parent.TryGetComponent(out BetterPlayerControl _better))
                {
                    _better.Resurrect.SetHeal();
                    pushCount++;
                    if (pushCount >= 5)
                    {
                        _heaith._hp = _better.Resurrect.GetHeal();
                        _better.Resurrect.gameObject.SetActive(false);
                        _better.Revive();
                        pushCount = 0;
                        _better.Resurrect.ResetHeel();
                    }
                }
            }
            else
            {
                _animator.SetTrigger("Attack");
                if (_jumpStatus.GetIsGrounded())
                {
                    _character.EnableMove = false;
                }
            }
        }

        // ジャンプ
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack_Air3_End"))
            {
                return;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Die"))
            {
                return;
            }

            if (_dasher.IsDashing)
            {
                _animator.SetTrigger("JumpAttack");
                _dasher.StopDash();
            }

            _animator.SetBool("IsJumping", true);
            _jumpStatus.StartJump();
            _character.EnableAttack = true;

            if (_heaith != null)
            {
                pushCount = 0;
                if (_heaith.HP < 0)
                {
                    if (_resurrection != null)
                    {
                        _resurrection.ResetHeel();
                    }
                }
            }
        }

        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Die"))
            {
                return;
            }

            _dasher.StartDash();
            _character.EnableMove = false;
            _character.EnableTurn = false;
            _character.EnableAttack = true;
        }

        // 弱攻撃
        if (_gamepad.GetButtonDown("X"))
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

        // 残像攻撃（技）
        {
            _skillManager.StartSkill();
        }

        // 死ぬ制御
        var health = _sprite.GetComponent<Health>();
        if (health.HP <= 0)
        {
            _resurrection.gameObject.SetActive(true);
            _character.IsDie = true;
        }

        //　足元のエフェクトの制御
        {
            float movingSpeed = Mathf.Abs(_gamepad.GetStickL().X) + Mathf.Abs(_gamepad.GetStickL().Y);
            _dustEmission.rateOverTime = _dustOverRate * Mathf.Clamp01(movingSpeed);
        }
        else
        {
            _dustEmission.rateOverTime = 0.0f;
        }
    }

    // ゲームパットのＩＤをセットする
    public void SetControllerIndex(int index)
    {
        _controllerIndex = index;
    }

    // ゲームパットの振動
    {
        _gamepad.AddRumble(timer, fadeTime, power);
    }

    // ゲームパットのＩＤをゲットする
    public Controller GetGamepad()
    {
        return _gamepad;
    }

    public void Revive()
    {
        _character.IsDie = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {       
            _heaith = collision.GetComponent<Health>();
        }
    }

    private void OnTriggerExi2Dt(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(_heaith.gameObject.name == collision.gameObject.tag)
            {
                _heaith = null;
            }
        }
    }
}
