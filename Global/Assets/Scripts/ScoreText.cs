using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();

        text.text = GameObject.Find("Score").GetComponent<Score>().ScoreSetting.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
