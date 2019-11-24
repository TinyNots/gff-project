using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Controller gamepad; // Gamepad instance

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Obtain the desired gamepad from GamepadManager
        gamepad = GamePadManager.Instance.GetGamepad(1);

        // Sample code to test button input and rumble
        if (gamepad.GetButtonDown("A"))
        {
            TestRumble();
            Debug.Log("A down");
        }

        transform.Translate(Vector2.right * gamepad.GetStickL().X * Time.deltaTime * 5f);
    }

    // Send some rumble events to the gamepad
    void TestRumble()
    {
        gamepad.AddRumble(0.5f, 0.1f, new Vector2(1f, 1f));
        //gamepad.AddRumble(2.5f, 0.2f, new Vector2(0.5f, 0.5f));
    }
}
