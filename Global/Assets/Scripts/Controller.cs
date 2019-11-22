using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerIndex player = PlayerIndex.One;
        GamePadState state = GamePad.GetState(player);
        if(state.IsConnected)
        {
            Debug.Log("Controller " + player + " is connected.");
        }
        else
        {
            Debug.Log("Controller " + player + " is not connected.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIndex player = PlayerIndex.One;
        GamePadState state = GamePad.GetState(player);

        Debug.Log(state.ThumbSticks.Left.X);

        transform.Translate(Vector2.right * state.ThumbSticks.Left.X * _speed * Time.deltaTime);
    }
}
