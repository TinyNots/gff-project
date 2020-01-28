using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RankingUI : MonoBehaviour
{
    public Text[] text = new Text[5];
    private Controller _gamePad;
    private int _controllerIndex = 1;


    // Start is called before the first frame update
    void Start()
    {
        _gamePad = GamePadManager.Instance.GetGamepad(_controllerIndex);

    }

    void Awake()
    {
        for (int i = 0; i < Leaderboard.entryCount; ++i)
        {
            var entry = Leaderboard.GetEntry(i);
        
            text[i].text = "    " + entry.name + "  WAVE" + entry.score;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_gamePad.GetButtonDown("X"))
        {
            SceneCtl.instance.NextScene(SceneCtl.SCENE_ID.TITLE);
        }
    }
}
