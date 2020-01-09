﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        // シーン遷移によるオブジェクト破棄をしない
        DontDestroyOnLoad(this.gameObject);
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("score:" + score);
    }

    public void AddScore(int val)
    {
        score += val;
    }

    public int ScoreSetting
    {
        set { score = value; }
        get { return score; }
    }
}