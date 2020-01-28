using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static string _playerName;
    private static int _score = 0;
    // Start is called before the first frame update

    void Start()
    {
        // シーン遷移によるオブジェクト破棄をしない
        DontDestroyOnLoad(this.gameObject);
        _score = 0;
        _playerName = " A A A A A ";
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("score:" + _score);
    }

    public void AddScore(int val)
    {
        _score += val;
    }

    public int ScoreSetting
    {
        set { _score = value; }
        get { return _score; }
    }

    public string PlayerName
    {
        set { _playerName = value; }
        get { return _playerName; }
    }
}
