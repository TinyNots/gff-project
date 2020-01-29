using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resurrection : MonoBehaviour
{
    private Slider _slider;
    [SerializeField]
    private float _value;
    [SerializeField]
    private float _speed = 1.0f;
    // バーの開始位置を固定するための変数
    private float _startTime;

    private float _healPoint;
    [SerializeField]
    private Slider _HP;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 0.5f;
        _healPoint = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        _startTime = Time.time;
    }

    private void PingPong()
    {
        _slider.value = Mathf.PingPong((Time.time - _startTime - 0.3f) * _speed, 2) - 1;
    }

    public void Recovery()
    {
        _healPoint = (Mathf.Abs(Mathf.Round(_slider.value * 10)));
    }

    public void GageAnimation()
    {
        if (Mathf.CeilToInt(_healPoint) == 0)
        {
            animator.SetBool("test", true);
        }
        else
        {
            animator.SetBool("test", false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        PingPong();
        GageAnimation();
        Debug.Log(Mathf.Round(_slider.value * 10));
        Recovery();
    }
}
