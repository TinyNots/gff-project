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

    private int _healPoint;
    private float _ratio;
    [SerializeField]
    private Animator animator = null;

    private void Awake()
    {
        _ratio = 0;
        _slider = GetComponent<Slider>();
        _slider.value = 0;
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
        _ratio = Mathf.Abs(Mathf.Round(_slider.value * 10));
        _healPoint += (int)(Mathf.Abs(Mathf.Round(_slider.value * 10)));
    }
    public void ResetHeel()
    {
        _healPoint = 0;
    }
    public void SetHeal()
    {
        _healPoint += (int)(Mathf.Abs(Mathf.Round(_slider.value * 10)));
    }
    public int GetHeal()
    {
        return _healPoint * 5;
    }

    public float Slider()
    {
        return _slider.value;
    }
    private void GageAnimation()
    {
        _ratio = Mathf.Abs(Mathf.Round(_slider.value * 10));
        if ((int)_ratio == 0)
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
    }
}
