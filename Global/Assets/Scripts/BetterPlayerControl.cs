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

    // Start is called before the first frame update
    void Start()
    {
        //_controllerIndex = 0;
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
        var health = transform.Find("Sprite").gameObject.GetComponent<Health>();
        if (health.HP <= 0)
        {
            _character.IsDie = true;
            _character.EnableMove = false;
        }
    }

    public void SetControllerIndex(int index)
    {
        _controllerIndex = index;
    }
}
