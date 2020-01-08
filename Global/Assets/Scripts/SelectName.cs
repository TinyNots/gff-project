using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    [SerializeField, Tooltip("登録できる名前の長さ")]
    private int _nameLength;
    // 表示する名前
    private Text _text;
    private Controller _gamePad;
	private List<string> _textList;
	private int _nowSetNameID;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = " ";
        for (int i = 0; i < _nameLength; i++)
        {
			_textList.Add("- ");
        }
		
        Test();
    }
    public void Test()
    {
        GamePadManager.Instance.GetGamepad(1).HaveTarget = true;
        _gamePad = GamePadManager.Instance.GetGamepad(1);
    }

	private void SetName()
	{
		// 決定ボタンで次の名前のIDに進む
		if (_gamePad.GetButtonDown("A"))
		{
			_nowSetNameID++;
		}
		// Cancelボタンで前の名前のIDに戻る
		else if (_gamePad.GetButtonDown("B"))
		{
			_nowSetNameID--;
		}
		// ID超え対策 
		if (_nowSetNameID > _nameLength - 1)
		{
			_nowSetNameID = _nameLength - 1;
		}
		else if(_nowSetNameID < 0)
		{
			_nowSetNameID = 0;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (_gamePad.GetStickL().Y > 0.5)
        {

            Debug.Log("上方向");
        }
        else if (_gamePad.GetStickL().Y < -0.5)
        {
            Debug.Log("下方向");
        }
    }
}
