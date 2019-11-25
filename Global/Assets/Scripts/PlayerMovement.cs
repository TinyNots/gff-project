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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float halfScreen = Screen.width / 2f;
        Debug.DrawLine(new Vector3(-halfScreen, 0), new Vector3(halfScreen, 0), Color.white);
        // Obtain the desired gamepad from GamepadManager
        gamepad = GamePadManager.Instance.GetGamepad(1);

        // Sample code to test button input and rumble
        if (gamepad.GetButtonDown("A"))
        {
            TestRumble();
            Debug.Log("A down");
        }

        //transform.Translate(Vector2.right * gamepad.GetStickL().X * Time.deltaTime * 5f);
        if (_isEableMove)
        {
            depth += _speed * gamepad.GetStickL().Y * Time.deltaTime;
        }

        depth = Mathf.Clamp(depth, -4.2f, 0.2f);

        // test jump code
        if (gamepad.GetButtonDown("A"))
        {
            //GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if(_isEableMove)
        {
            Vector2 movement = new Vector2(gamepad.GetStickL().X, gamepad.GetStickL().Y) * _speed * Time.deltaTime;
            transform.Translate(movement);

            Vector2 position = transform.position;
            position.y = Mathf.Clamp(position.y, -4.2f, 0.2f);
            transform.position = position;
        }
    }

    // Send some rumble events to the gamepad
    void TestRumble()
    {
        gamepad.AddRumble(0.5f, 0.0f, new Vector2(1f, 1f));
        //gamepad.AddRumble(2.5f, 0.2f, new Vector2(0.5f, 0.5f));
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
}
