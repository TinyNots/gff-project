using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{

    private static HighScoreManager _highScoreMng;
    private const int rankingLen = 5;

    public static HighScoreManager Instance
    {
        get
        {
            if (_highScoreMng == null)
            {
                _highScoreMng = new GameObject("HighScoreManager").AddComponent<HighScoreManager>();
            }
            return _highScoreMng;
        }
    }

    void Awake()
    {
        if (_highScoreMng == null)
        {
            _highScoreMng = this;
        }
        else if (_highScoreMng != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighScore(string name, int score)
    {
        List<Score> highScores = new List<Score>();

        int i = 1;
        while (i <= rankingLen && PlayerPrefs.HasKey("HighScore" + i + "score"))
        {
            Score temp = new Score();
            temp.ScoreSetting = PlayerPrefs.GetInt("HighScore" + i + "score");
            highScores.Add(temp);
            i++;
        }
        if (highScores.Count == 0)
        {
            Score _temp = new Score();
            _temp.ScoreSetting = score;
            highScores.Add(_temp);
        }
        else
        {
            for (i = 1; i <= highScores.Count && i <= rankingLen; i++)
            {
                if (score > highScores[i - 1].ScoreSetting)
                {
                    Score _temp = new Score();
                    _temp.ScoreSetting = score;
                    highScores.Insert(i - 1, _temp);
                    break;
                }
                if (i == highScores.Count && i < rankingLen)
                {
                    Score _temp = new Score();
                    _temp.ScoreSetting = score;
                    highScores.Add(_temp);
                    break;
                }
            }
        }

        i = 1;
        while (i <= rankingLen && i <= highScores.Count)
        {
            PlayerPrefs.SetInt("HighScore" + i + "score", highScores[i - 1].ScoreSetting);
            i++;
        }

    }

    public List<Score> GetHighScore()
    {
        List<Score> highScores = new List<Score>();

        int i = 1;
        while (i <= rankingLen && PlayerPrefs.HasKey("HighScore" + i + "score"))
        {
            Score temp = new Score();
            temp.ScoreSetting = PlayerPrefs.GetInt("HighScore" + i + "score");
            highScores.Add(temp);
            i++;
        }

        return highScores;
    }

    public void ClearLeaderBoard()
    {
        List<Score> highScores = GetHighScore();

        for (int i = 1; i <= highScores.Count; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i + "score");
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
