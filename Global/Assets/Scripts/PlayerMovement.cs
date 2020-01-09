using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Controller gamepad; // Gamepad instance
    public float depth = 0f;
    [SerializeField]
    private float _speed = 5.0f;
    private bool _isEableMove = true;
    [SerializeField]
    private Animator animator;
    private int _controllerIndex;
    [SerializeField]
    [Range(0, 1f)]
    private float _speedPercent = 0.5f;
    [SerializeField]
    private GameObject _sprite;
    [SerializeField]
    private Jumper _jumpStatus;
    [SerializeField]
    private Character _character;

    // Use this for initialization
    void Start()
    {
        _controllerIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_controllerIndex == 0)
        {
            return;
        }

        float halfScreen = Screen.width / 2f;
        Debug.DrawLine(new Vector3(-halfScreen, 2f), new Vector3(halfScreen, 2f), Color.white);
        // Obtain the desired gamepad from GamepadManager
        gamepad = GamePadManager.Instance.GetGamepad(_controllerIndex);

        if (_character.EnableMove)
        {
            depth += _speed * gamepad.GetStickL().Y * Time.deltaTime;
            animator.SetFloat("Speed", Mathf.Abs(gamepad.GetStickL().X + gamepad.GetStickL().Y));
        }

        depth = Mathf.Clamp(depth, -4.2f, 2f);

        if (gamepad.GetButtonDown("X"))
        {
            animator.SetTrigger("Attack");
            _character.EnableMove = false;
        }

        if (_character.EnableMove)
        {
            if (gamepad.GetButtonDown("B"))
            {
                animator.SetBool("IsHurt", true);
                gamepad.AddRumble(0.5f, 0, new Vector2(1f, 1f));
                _character.EnableMove = false;
            }
        }

        if (GamePadManager.Instance.GetGamepad(_controllerIndex).GetStickL().X < -0.01f)
        {
            _sprite.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (GamePadManager.Instance.GetGamepad(_controllerIndex).GetStickL().X > 0.01f)
        {
            _sprite.transform.localRotation = Quaternion.Euler(0f, 0, 0f);
        }

        if(GamePadManager.Instance.GetGamepad(_controllerIndex).GetButton("A"))
        {
            animator.SetBool("IsJumping", true);
            _jumpStatus.StartJump();
        }
    }

    private void FixedUpdate()
    {
        if (_controllerIndex == 0)
        {
            return;
        }

        if (_character.EnableMove)
        {
            Vector2 movement = new Vector2(gamepad.GetStickL().X, gamepad.GetStickL().Y * _speedPercent) * _speed * Time.deltaTime;
            transform.Translate(movement);

            Vector2 position = transform.position;
            position.y = Mathf.Clamp(position.y, -4.2f, 2f);
            transform.position = position;
        }
    }

    public float GetDepth()
    {
        return depth;
    }

    public bool EableMove
    {
        get { return _isEableMove; }
        set { _isEableMove = value; }
    }

    public void SetControllerIndex(int index)
    {
        _controllerIndex = index;
    }

    public int GetControllerIndex()
    {
        return _controllerIndex;
    }

    public void ResetJump()
    {
        animator.SetBool("IsJumping", false);
    }

    public void StopMovement()
    {
        _character.EnableMove = false;
    }
}
