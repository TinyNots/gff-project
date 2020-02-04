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
    [SerializeField]
    private Animator animator = null;
    private Character _character;
    public Character GetChara
    {
        get { return _character; }
    }

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

    public void SetPlayer(Character character)
    {
        _character = character;
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
        _healPoint += (int)(Mathf.Abs(Mathf.Round(_slider.value * 10)));
    }
    public void ResetHP()
    {
        _healPoint = 0;
    }
    public int SetHeal()
    {
        return _healPoint;
    }

    private void GageAnimation()
    {
        if (_healPoint == 0)
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
