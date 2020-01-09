using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField, Tooltip("無効化するオブジェクト")]
    private GameObject[] _noActiveObj;
    [SerializeField]
    private GameObject[] Buttons;
    Controller _gamepad;
    [SerializeField]
    private List<FadeUI> _fade = new List<FadeUI>();
    [SerializeField]
    private TypefaceAnimator _typeface = null;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        GamePadManager.Instance.GetGamepad(1).HaveTarget = true;
        _gamepad = GamePadManager.Instance.GetGamepad(1);
    }

    public void PushButton()
    {
        foreach(var button in Buttons)
        {
            button.SetActive(true);
        }
        foreach (var obj in _noActiveObj)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Startボタンを押したら
        if (_gamepad.GetButtonUp("Start"))
        {
            foreach (var fade in _fade)
            {
                fade.Active = true;
            }
            _typeface.enabled = false;
        }
    }
}
