using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    [SerializeField, Tooltip("変更する色")]
    private Color _color = Color.white;
    private List<Image> _imageList = new List<Image>();
    // 自身の遷移情報を持ったボタン
    private List<SceneButton> _buttonList = new List<SceneButton>();
    private int _stageCnt;
    private int _nowCnt;
    // Start is called before the first frame update
    void Start()
    {
        _stageCnt = 0;
        _nowCnt = 0;
        Image image = null;
        SceneButton button = null;
        foreach(RectTransform child in transform)
        {
            button = child.GetComponent<SceneButton>();
            if(button != null)
            {
                _buttonList.Add(button);
                _stageCnt++;
            }
            image = child.GetComponent<Image>();      
            if(image != null)
            {
                _imageList.Add(image);
            }
        }
    }

    private void ChangeButton()
    {
        if(_buttonList.Count <= 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_nowCnt > 0)
            {
                _nowCnt--;
            }
            else
            {
                _nowCnt = _stageCnt;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_nowCnt < _stageCnt)
            {
                _nowCnt++;
            }
            else
            {
                _nowCnt = 0;
            }
        }

        SetColor();

        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.A)) && _nowCnt > 0)
            {
                _buttonList[_nowCnt - 1].ChangeScene();
            }
        }
    }

    private void SetColor()
    {
        // イメージリストの数が0以下なら処理しない
        if(_imageList.Count <= 0)
        {
            return;
        }
        foreach(Image image in _imageList)
        {
            image.color = UnityEngine.Color.white;
        }
        if(_nowCnt > 0)
        {
            _imageList[_nowCnt - 1].color = _color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeButton();
    }
}
