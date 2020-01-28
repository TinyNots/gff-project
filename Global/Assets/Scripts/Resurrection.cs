using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resurrection : MonoBehaviour
{
    private Slider _slider;
    private float _value;
    [SerializeField]
    private float _speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _value = 0.0f;
        _slider.value = _value;
    }
    
    private void PingPong()
    {

        _slider.value = Mathf.PingPong(Time.time * _speed, 1);
    }

    // Update is called once per frame
    void Update()
    {
        PingPong();
    }
}
