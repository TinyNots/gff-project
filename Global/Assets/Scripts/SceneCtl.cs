using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtl : MonoBehaviour
{
    public static SceneCtl instance = null;


    public enum SCENE_ID
    {
        TITLE,
        STAGE1,
        RESULT,
        MAX
    }
    // シーンの名前
    private string _sceneName;
    // 演出フラグ
    private bool _isDirecting;
    private List<RectTransform> _rectTrans = new List<RectTransform>();

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
    }

    public void open()
    {

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
            case SCENE_ID.STAGE1:
                _sceneName = "Stage1";
                break;
            case SCENE_ID.RESULT:
                _sceneName = "ResultScene";
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
    }
}
