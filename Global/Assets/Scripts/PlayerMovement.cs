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


        //transform.Translate(Vector2.right * gamepad.GetStickL().X * Time.deltaTime * 5f);
        if (_isEableMove)
        {
            depth += _speed * gamepad.GetStickL().Y * Time.deltaTime;
            animator.SetFloat("Speed", Mathf.Abs(gamepad.GetStickL().X + gamepad.GetStickL().Y));
        }

        depth = Mathf.Clamp(depth, -4.2f, 2f);

        // test jump code
        if (gamepad.GetButtonDown("A"))
        {
            //GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);\
        }

        if(_isEableMove)
        {
            if (gamepad.GetButtonDown("X"))
            {
                animator.SetBool("IsAttacking", true);
                _isEableMove = false;
            }

            if (gamepad.GetButtonDown("B"))
            {
                animator.SetBool("IsHurt", true);
                gamepad.AddRumble(0.5f, 0, new Vector2(0.8f, 0.8f));
                _isEableMove = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_controllerIndex == 0)
        {
            return;
        }

        if (_isEableMove)
        {
            Vector2 movement = new Vector2(gamepad.GetStickL().X, gamepad.GetStickL().Y) * _speed * Time.deltaTime;
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
}
