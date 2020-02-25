using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberTransition : MonoBehaviour
{
    [SerializeField]
    private Score _score;
    [SerializeField]
    private WaveMng _waveReady;
    private Text _text;
    private Animator _animator;

    void Start()
    {
        _text = GetComponent<Text>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // デバック用
        if(Input.GetKeyDown(KeyCode.F2))
        {
            _animator.SetTrigger("Change");

        }
        if (_waveReady.GetFlag())
        {   
            _animator.SetTrigger("Change");
        }
    }

    public void IncreaseNumber()
    {
        _text.text = _score.ScoreSetting.ToString();
    }
}
