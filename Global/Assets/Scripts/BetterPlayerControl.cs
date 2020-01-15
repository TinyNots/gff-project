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

    private SpriteRenderer _renderer;
    private Shader ShaderGUItext;
    private Shader shaderSpritesDafult;

    // Start is called before the first frame update
    void Start()
    {
        //_controllerIndex = 0;
        _renderer = _sprite.GetComponent<SpriteRenderer>();
        ShaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDafult = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
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
        if(_gamepad.GetButtonDown("X"))
        {
            _animator.SetTrigger("Attack");
            if(_jumpStatus.GetIsGrounded())
            {
                _character.EnableMove = false;
            }
        }

        if(_gamepad.GetButtonDown("A") && _character.EnableMove)
        {
            _animator.SetBool("IsJumping", true);
            _jumpStatus.StartJump();
        }

        if(_gamepad.GetButtonDown("B") && !_character.IsHurt && _jumpStatus.GetIsGrounded())
        {
            _character.IsHurt = true;
            _character.EnableMove = false;
        }

        if(_gamepad.GetButtonDown("Y"))
        {
            FindObjectOfType<HitStop>().Stop(0.06f);
            _renderer.material.shader = ShaderGUItext;
            _renderer.color = Color.white;
            StartCoroutine(Normal(0.06f));
        }

        var health = _sprite.GetComponent<Health>();
        if (health.HP <= 0)
        {
            _character.IsDie = true;
            _character.EnableMove = false;
            _character.EnableTurn = false;
        }
    }

    public void SetControllerIndex(int index)
    {
        _controllerIndex = index;
    }

    private IEnumerator Normal(float time)
    {
        yield return new WaitForSeconds(time);
        _renderer.material.shader = shaderSpritesDafult;
        _renderer.color = Color.white;
    }
}
