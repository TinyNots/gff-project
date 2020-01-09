using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    private Controller _gamePad;
    private int _playerTotalIndex;
    private int _controllerIndex;
    [SerializeField, Tooltip("ロックイメージ")]
    private GameObject[] _lockImage;
    public int PlayerTotalIndex
    {
        get { return _playerTotalIndex; }
    }
	private bool _sceneFlag;

	// Start is called before the first frame update
	void Start()
    {
		_sceneFlag = false;
        _playerTotalIndex = 0;
        // スタートボタンを押したか
        int currentIndex = 1;
        if (currentIndex != 0 && !GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget)
        {
            GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget = true;
            _lockImage[_playerTotalIndex].SetActive(false);
            _playerTotalIndex++;
        }
        _gamePad = GamePadManager.Instance.GetGamepad(1);
    }

    void CheckSelect()
    {
        if (_playerTotalIndex < 4)
        {
            int currentIndex = GamePadManager.Instance.GetControllerByButton("Start");

            if (currentIndex != 0 && !GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget)
            {
                GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget = true;
                _lockImage[_playerTotalIndex].SetActive(false);
                _playerTotalIndex++;
            }
        }
    }

    private void StartGame()
    {
        if (_gamePad.GetButtonDown("Start"))
        {
            Debug.Log("シーン遷移します");
			if(!_sceneFlag)
			{
				_sceneFlag = true;
				Invoke("SetMode", 2.0f);
			}
        }
    }


	public void SetMode()
	{
		SceneCtl.instance.SetNextScene(SceneCtl.SCENE_ID.GAME);
		SceneCtl.instance.SetDirect(SceneCtl.DIRECT.START);
	}

	// Update is called once per frame
	void Update()
    {
        CheckSelect();
    }

    private void LateUpdate()
    {
        StartGame();
    }
}
