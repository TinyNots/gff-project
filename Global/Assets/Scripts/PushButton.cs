using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushButton : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();
    private Image _image;

    Controller _gamepad;


    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Push()
    {
        if (GamePadManager.Instance.GetGamepad(1) == null)
        {
            return;
        }
        else if (_gamepad == null)
        {
            _gamepad = GamePadManager.Instance.GetGamepad(1);
        }
        if (_gamepad.GetButton("Start"))
        {
            // 押し続けている間、イメージを変更する
            _image.sprite = _spriteList[1];
        }

        // Startボタンを押したら
        if (_gamepad.GetButtonUp("Start"))
        {
            _image.sprite = _spriteList[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Push();
    }

    private void LateUpdate()
    {
    }
}
