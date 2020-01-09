using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    Controller _gamepad;
    [SerializeField]
    private List<FadeUI> _fade = new List<FadeUI>();
    [SerializeField]
    private TypefaceAnimator _typeface = null;
    private int i = 0;
	private bool _sceneFlag;
    // Start is called before the first frame update
    void Start()
    {
		_sceneFlag = false;
        GamePadManager.Instance.GetGamepad(1).HaveTarget = true;
        _gamepad = GamePadManager.Instance.GetGamepad(1);
    }

    public void PushButton()
    {
        // Startボタンを押したら
        if (_gamepad.GetButtonUp("Start"))
        {
            foreach (var fade in _fade)
            {
                fade.Active = true;
            }
			_typeface.enabled = false;
			if(!_sceneFlag)
			{
				_sceneFlag = true;
				Invoke("SetMode", 1.0f);
			}
        }
    }

	public void SetMode()
	{
		SceneCtl.instance.SetNextScene(SceneCtl.SCENE_ID.SELECT);
		SceneCtl.instance.SetDirect(SceneCtl.DIRECT.START);
	}

    // Update is called once per frame
    void Update()
    {
		PushButton();
    }
}
