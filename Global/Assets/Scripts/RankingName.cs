using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingName : MonoBehaviour
{
    [SerializeField, Tooltip("ランキングネーム")]
    private Text[] _texts;
    private int _num;
    private Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    public void SetRunking(Text[] texts)
    {
        _texts = texts;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
