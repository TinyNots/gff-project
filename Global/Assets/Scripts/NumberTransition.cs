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

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
