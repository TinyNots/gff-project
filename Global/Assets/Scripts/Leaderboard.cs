using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class Leaderboard
{
    //Top 5
    public const int entryCount = 5;

    public struct ScoreEntry
    {
        public string name;
        public int score;

        public ScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    private static List<ScoreEntry> _entryInstance;

    private static List<ScoreEntry> Entries
    {
        get
        {
            if (_entryInstance == null)
            {
                _entryInstance = new List<ScoreEntry>();
                LoadScores();
            }
            return _entryInstance;
        }
    }

    private const string PlayerPrefsBaseKey = "leaderboard";

    
    private static void SortScores()
    {
        _entryInstance.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private static void LoadScores()
    {
        _entryInstance.Clear();

        for (int i = 0; i < entryCount; ++i)
        {
            ScoreEntry entry;
            entry.name = PlayerPrefs.GetString(PlayerPrefsBaseKey + "[" + i + "].name", "");
            entry.score = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].score", 0);
            _entryInstance.Add(entry);
        }

        SortScores();
    }

    private static void SaveScores()
    {
        for (int i = 0; i < entryCount; ++i)
        {
            var entry = _entryInstance[i];
            PlayerPrefs.SetString(PlayerPrefsBaseKey + "[" + i + "].name", entry.name);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].score", entry.score);
        }
    }

    public static ScoreEntry GetEntry(int index)
    {
        return Entries[index];
    }

    public static void Record(string name, int score)
    {
        Entries.Add(new ScoreEntry(name, score));
        SortScores();
        Entries.RemoveAt(Entries.Count - 1);
        SaveScores();
    }
}