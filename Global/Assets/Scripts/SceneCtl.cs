using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtl : MonoBehaviour
{
    public static SceneCtl instance = null;

    public List<RectTransform> _imageList = new List<RectTransform>();

    [SerializeField]
    private List<AudioClip> _clipList = new List<AudioClip>();
    public enum SCENE_ID
    {
        TITLE,
		SELECT,
        GAME,
        RESULT,
        MAX
    }

	// 演出の状態
	public enum DIRECT
	{
		NON,
		START,
		END,
		MAX
	}



	SCENE_ID _id;
	DIRECT _mode;
    // シーンの名前
    private string _sceneName;
    // 演出フラグ
    private bool _isDirecting;
    private List<RectTransform> _rectTrans = new List<RectTransform>();
	public float _speed = 5f;
	private float _nowTime = 0f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update

    void Start()
    {
		_id = SCENE_ID.TITLE;
	}

	public void SetNextScene(SCENE_ID id)
	{
		_id = id;
	}

	public void SetDirect(DIRECT mode)
	{
		_mode = mode;
	}

	public void downOpen()
    {
		Vector2 point1 = _imageList[0].localPosition;
		Vector2 point2 = _imageList[1].localPosition;
		_nowTime += Time.deltaTime;
		_imageList[0].localPosition = Vector3.Lerp(_imageList[0].localPosition, new Vector3(-500, point1.y, 0), Time.deltaTime * _speed);
		_imageList[1].localPosition = Vector3.Lerp(_imageList[1].localPosition, new Vector3(500, point2.y, 0), Time.deltaTime * _speed);
		if(_nowTime > _speed * 2)
		{
			NextScene(_id);
			_imageList[0].localPosition = new Vector3(-500, point1.y, 0);
			_imageList[1].localPosition = new Vector3(500, point1.y, 0);
			_mode = DIRECT.END;
			_nowTime = 0;
		}
	}

	public void Open()
	{
		Vector2 point1 = _imageList[0].localPosition;
		Vector2 point2 = _imageList[1].localPosition;
		_nowTime += Time.deltaTime;
		_imageList[0].localPosition = Vector3.Lerp(_imageList[0].localPosition, new Vector3(-1500, point1.y, 0), Time.deltaTime * _speed);
		_imageList[1].localPosition = Vector3.Lerp(_imageList[1].localPosition, new Vector3(1500, point2.y, 0), Time.deltaTime * _speed);
		if (_nowTime > _speed * 2)
		{
			_imageList[0].localPosition = new Vector3(-1500, point1.y, 0);
			_imageList[1].localPosition = new Vector3(1500, point1.y, 0);
			_mode = DIRECT.NON;
			_nowTime = 0;
		}
	}

	public void Directing()
	{
		switch (_mode)
		{
			case DIRECT.START:
				downOpen();
				break;
			case DIRECT.END:
				Open();
				break;
			case DIRECT.NON:
			case DIRECT.MAX:
			default:
				break;
		}
	}

	public void NextScene(SCENE_ID id)
    {
        _sceneName = "";
		// SCENEの登録IDで判断する
		switch (id)
		{
			case SCENE_ID.TITLE:
				_sceneName = "TitleScene";
                break;
			case SCENE_ID.SELECT:
				_sceneName = "SelectScene";
				break;
			case SCENE_ID.GAME:
				_sceneName = "scene";
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlayBGM(_clipList[1]);
				break;
			case SCENE_ID.RESULT:
				_sceneName = "ResultScene";
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlayBGM(_clipList[0]);
                break;

			case SCENE_ID.MAX:
			default:
				break;
		}
		if (_sceneName != "")
        {
            SceneManager.LoadScene(_sceneName);
        }
        else
        {
            Debug.Log("存在しないシーンを呼ぼうとしています。");
        }
	}

	// Update is called once per frame
	void Update()
    {
		Directing();
	}
}