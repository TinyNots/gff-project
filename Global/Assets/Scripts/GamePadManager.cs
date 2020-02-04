using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadManager : MonoBehaviour
{
    [SerializeField]
    private int _gamepadCount = 4;

    private List<Controller> _gamepads;
    private static GamePadManager _instance;

    void Awake()
    {
        _instance = this;
        if (_instance == null && _instance != this)
        {
            //Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(_instance);

        _gamepadCount = Mathf.Clamp(_gamepadCount, 1, 4);

        _gamepads = new List<Controller>();

        for (int i = 0; i < _gamepadCount; i++)
        {
            _gamepads.Add(new Controller(i));
        }
    }

    void Update()
    {
        Refresh();

        foreach (Controller gamepad in _gamepads)
        {
            gamepad.Update();
        }
    }

    public static GamePadManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("[GamepadManager]: Instance does not exist!");
                return null;
            }
            return _instance;
        }
    }

    public void Refresh()
    {
        foreach(Controller gamePad in _gamepads)
        {
            gamePad.Refresh();
        }
    }

    public Controller GetGamepad(int index)
    {
        for (int i = 0; i < _gamepads.Count;)
        {
            if (_gamepads[i].Index == (index - 1))
            {
                return _gamepads[i];
            }
            else
            {
                i++;
                continue;
            }
        }

        Debug.LogError("[GamepadManager]: " + index + " is not a valid gamepad index!");
        return null;
    }

    public int ConnectedTotal()
    {
        int total = 0;

        for (int i = 0; i < _gamepads.Count; i++)
        {
            if (_gamepads[i].IsConnected)
            {
                total++;
            }
        }

        return total;
    }

    public bool GetButtonAny(string button)
    {
        for (int i = 0; i < _gamepads.Count; ++i)
        {
            if (_gamepads[i].IsConnected && _gamepads[i].GetButton(button))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetButtonDownAny(string button)
    {
        for (int i = 0; i < _gamepads.Count; ++i)
        {
            if (_gamepads[i].IsConnected && _gamepads[i].GetButtonDown(button))
            {
                return true;
            }
        }
        return false;
    }

    public int GetControllerByButton(string button)
    {
        for (int i = 0; i < _gamepads.Count; ++i)
        {
            if (_gamepads[i].IsConnected && _gamepads[i].GetButtonDown(button))
            {
                return i + 1;
            }
        }
        return 0;
    }

    public void RumbleAll(float timer,float fadeTime,Vector2 power)
    {
        for (int i = 0; i < _gamepads.Count; ++i)
        {
            _gamepads[i].AddRumble(timer, fadeTime, power);
        }
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < _gamepadCount; i++) 
        {
            _gamepads[i].StopRumble();
        }
    }
}
