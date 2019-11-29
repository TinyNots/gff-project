using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

// コントローラのボタン状態
public struct B_State
{
    public ButtonState oldState;
    public ButtonState nowState;
}

// コントローラのトリガー状態
public struct T_State
{
    public float oldValue;
    public float nowValue;
}

// 振動イベント
class Rumble
{
    public float timer;     // 振動の時間
    public float fadeTime;  // フェードアウトの時間（秒）
    public Vector2 power;   // 振動の強度

    public void Update()
    {
        this.timer -= Time.deltaTime;
    }
}

public class Controller
{
    private GamePadState _oldState;
    private GamePadState _state;

    private int _gamepadIndex;
    private PlayerIndex _playerIndex;
    private List<Rumble> _rumbleEvents;

    private Dictionary<string,B_State> _inputMap;

    private B_State A, B, X, Y;
    private B_State DPad_Up, DPad_Down, DPad_Left, DPad_Right;

    private B_State Guide;
    private B_State Back, Start;
    private B_State L3, R3;
    private B_State LB, RB;
    private T_State LT, RT;

    private bool _haveTarget;

    // コンストラクタ
    public Controller(int index)
    {
        _gamepadIndex = index;
        _playerIndex = (PlayerIndex)_gamepadIndex;

        _inputMap = new Dictionary<string, B_State>();
        _rumbleEvents = new List<Rumble>();
    }

    public void Update()
    {
        _state = GamePad.GetState(_playerIndex);

        if(_state.IsConnected)
        {
            A.nowState = _state.Buttons.A;
            B.nowState = _state.Buttons.B;
            X.nowState = _state.Buttons.X;
            Y.nowState = _state.Buttons.Y;

            DPad_Up.nowState = _state.DPad.Up;
            DPad_Down.nowState = _state.DPad.Down;
            DPad_Left.nowState = _state.DPad.Left;
            DPad_Right.nowState = _state.DPad.Right;

            Guide.nowState = _state.Buttons.Guide;
            Back.nowState = _state.Buttons.Back;
            Start.nowState = _state.Buttons.Start;
            L3.nowState = _state.Buttons.LeftStick;
            R3.nowState = _state.Buttons.RightStick;
            LB.nowState = _state.Buttons.LeftShoulder;
            RB.nowState = _state.Buttons.RightShoulder;

            LT.nowValue = _state.Triggers.Left;
            RT.nowValue = _state.Triggers.Right;

            UpdateInputMap();
            HandleRumble();
        }
    }

    public void Refresh()
    {
        _oldState = _state;

        if (_state.IsConnected)
        {
            A.oldState = _oldState.Buttons.A;
            B.oldState = _oldState.Buttons.B;
            X.oldState = _oldState.Buttons.X;
            Y.oldState = _oldState.Buttons.Y;

            DPad_Up.oldState = _oldState.DPad.Up;
            DPad_Down.oldState = _oldState.DPad.Down;
            DPad_Left.oldState = _oldState.DPad.Left;
            DPad_Right.oldState = _oldState.DPad.Right;

            Guide.oldState = _oldState.Buttons.Guide;
            Back.oldState = _oldState.Buttons.Back;
            Start.oldState = _oldState.Buttons.Start;
            L3.oldState = _oldState.Buttons.LeftStick;
            R3.oldState = _oldState.Buttons.RightStick;
            LB.oldState = _oldState.Buttons.LeftShoulder;
            RB.oldState = _oldState.Buttons.RightShoulder;

            LT.oldValue = _oldState.Triggers.Left;
            RT.oldValue = _oldState.Triggers.Right;

            UpdateInputMap();
        }
    }

    private void UpdateInputMap()
    {
        _inputMap["A"] = A;
        _inputMap["B"] = B;
        _inputMap["X"] = X;
        _inputMap["Y"] = Y;

        _inputMap["DPad_Up"] = DPad_Up;
        _inputMap["DPad_Down"] = DPad_Down;
        _inputMap["DPad_Left"] = DPad_Left;
        _inputMap["DPad_Right"] = DPad_Right;

        _inputMap["Guide"] = Guide;
        _inputMap["Back"] = Back;
        _inputMap["Start"] = Start;

        _inputMap["L3"] = L3;
        _inputMap["R3"] = R3;

        _inputMap["LB"] = LB;
        _inputMap["RB"] = RB;
    }

    private void HandleRumble()
    {
        if (_rumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(0f, 0f);

            for (int i = 0; i < _rumbleEvents.Count; i++)
            {
                _rumbleEvents[i].Update();

                _rumbleEvents[i].Update();

                if (_rumbleEvents[i].timer > 0)
                {
                    // Calculate current power
                    float timeLeft = Mathf.Clamp(_rumbleEvents[i].timer / _rumbleEvents[i].fadeTime, 0f, 1f);
                    currentPower = new Vector2(Mathf.Max(_rumbleEvents[i].power.x * timeLeft, currentPower.x),
                                               Mathf.Max(_rumbleEvents[i].power.y * timeLeft, currentPower.y));

                    GamePad.SetVibration(_playerIndex, currentPower.x, currentPower.y);
                }
                else
                {
                    // Cancel out any phantom vibration
                    GamePad.SetVibration(_playerIndex, 0.0f, 0.0f);

                    // Remove expired event
                    _rumbleEvents.Remove(_rumbleEvents[i]);
                }
            }
        }
    }

    // 現在のIDを返す
    public int Index
    {
        get { return _gamepadIndex; }
    }

    // 接続状態を返す
    public bool IsConnected
    {
        get { return _state.IsConnected; }
    }

    public bool GetButton(string button)
    {
        return _inputMap[button].nowState == ButtonState.Pressed ? true : false;
    }

    public bool GetButtonDown(string button)
    {
        return (_inputMap[button].nowState == ButtonState.Pressed &&
                _inputMap[button].oldState == ButtonState.Released) ? true : false;
    }

    // コントローラに振動のイベントを追加する
    public void AddRumble(float timer, float fadeTime, Vector2 power)
    {
        Rumble rumble = new Rumble();

        rumble.timer = timer;
        rumble.power = power;
        rumble.fadeTime = fadeTime;

        _rumbleEvents.Add(rumble);
    }

    public GamePadThumbSticks.StickValue GetStickL()
    {
        return _state.ThumbSticks.Left;
    }

    public GamePadThumbSticks.StickValue GetStickR()
    {
        return _state.ThumbSticks.Right;
    }

    public float GetTriggerL()
    {
        return _state.Triggers.Left;
    }

    public float GetTrggerR()
    {
        return _state.Triggers.Right;
    }

    public bool GetTriggerTapL()
    {
        return (LT.nowValue >= 0.1f && LT.oldValue == 0f) ? true : false;
    }

    public bool GetTriggerTapR()
    {
        return (RT.nowValue >= 0.1f && RT.oldValue == 0f) ? true : false;
    }
}