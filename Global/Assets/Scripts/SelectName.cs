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
    private int _controllerIndex = 1;
    [SerializeField]
    private RankingUI _rankingUI;
    // 名前の全体のリスト
    private List<string> _nameList = new List<string>();
    private int _nowSetNameID;
    // 名前の決定
    private bool _decision;
    private char[] _names;
    private int _nowCharID;
    private string _textData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Start is called before the first frame update
    void Start()
    {
        _nowCharID = 0;
        _names = _textData.ToCharArray();

        for(int i = 0; i < _textData.Length; i++)
        {
            Debug.Log(_names[i]);
        }
        _nowSetNameID = 0;
        _text = GetComponent<Text>();
        _text.text = "";
        for (int i = 0; i < _nameLength; i++)
        {
			_nameList.Add("-");
        }
        SetText();
        Test();
        _gamePad = GamePadManager.Instance.GetGamepad(_controllerIndex);
    }
    public void Test()
    {
        //GamePadManager.Instance.GetGamepad(1).HaveTarget = true;
        //_gamePad = GamePadManager.Instance.GetGamepad(1);
    }

    public void SetText()
    {
        string str = "";
        for(int i = 0; i < _nameLength; i++)
        {
            str = str + _nameList[i] + " ";
        }
        _text.text = " " + str;
    }


	private void SetName()
	{
        if(_nameList[_nowSetNameID] == "-")
        {
            _nameList[_nowSetNameID] = "A";
        }
        // デバック
        Debug.Log(_names[_nowCharID].ToString());
        _nameList[_nowSetNameID] = _names[_nowCharID].ToString();

        //// 決定ボタンで次の名前のIDに進む
        //if (_gamePad.GetButtonDown("A"))
        //{
        //    _nowSetNameID++;
        //}
        //// Cancelボタンで前の名前のIDに戻る
        //else if (_gamePad.GetButtonDown("B"))
        //{
        //    _nowSetNameID--;
        //}

        if(Input.GetKeyDown(KeyCode.D))
        {
            _nowSetNameID++;
            if (_nowSetNameID > _nameLength - 1)
            {
                _nowSetNameID = 0;
                var tmpScore = GameObject.Find("Score").GetComponent<Score>();
                tmpScore.PlayerName = _text.text;
                Leaderboard.Record(tmpScore.PlayerName, tmpScore.ScoreSetting);
                _rankingUI.gameObject.SetActive(true);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            _nowSetNameID--;
            _nowCharID = 0;
        }
        if (_gamePad.GetButtonDown("X"))
        {
            _nowSetNameID++;
            if (_nowSetNameID > _nameLength - 1)
            {
                _nowSetNameID = 0;
                var tmpScore = GameObject.Find("Score").GetComponent<Score>();
                tmpScore.PlayerName = _text.text;
                Leaderboard.Record(tmpScore.PlayerName, tmpScore.ScoreSetting);
                _rankingUI.gameObject.SetActive(true);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
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
        // 設定されたテキストを表示する
        SetText();
    }

    private void LateUpdate()
    {
        //if (_gamePad.GetStickL().Y > 0.5)
        //{
        //    Debug.Log("上方向");
        //}
        //else if (_gamePad.GetStickL().Y < -0.5)
        //{
        //    Debug.Log("下方向");
        //}
        SetName();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _nowCharID++;
            Debug.Log("上入力");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _nowCharID--;
            Debug.Log("下入力");
        }
        if (_gamePad.GetButtonDown("DPad_Down"))
        {
            _nowCharID++;
        }
        else if (_gamePad.GetButtonDown("DPad_Up"))
        {
            _nowCharID--;

        }


        if (_nowCharID > 25)
        {
            _nowCharID = 0;
        }
        else if (_nowCharID < 0)
        {
            _nowCharID = 25;
        }
    }
}
