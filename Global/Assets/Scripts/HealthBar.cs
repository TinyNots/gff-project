using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float maxhp;
    private GameObject player;
    Health health;
    public int playerCnt;
    Slider slider;
    private float _oldSliderVal;
    [SerializeField]
    private Slider _oldSlider;
    private bool _moveOldSlider;
    private float _moveTiming = 1f;
    private float _sliderChgTime;
    [SerializeField]
    private GameObject _bg;
    int flashCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if(slider != null)
        {
            _oldSlider.value = slider.value;
            _oldSliderVal = _oldSlider.value;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (slider == null)
        {
            return;
        }

        if (player == null)
        {
            var tmpString = "Player " + playerCnt;
            player = GameObject.Find(tmpString);
            if (player != null)
            {
                health = player.transform.Find("Sprite").GetComponent<Health>();
                maxhp = health.HP;
                for (int a = 0; a < transform.childCount; a++)
                {
                    transform.GetChild(a).gameObject.SetActive(true);
                }
            }
        }

        if(health == null)
        {
            return;
        }
 
        slider.value = health._hp / maxhp;
        if (slider.value != _oldSliderVal)
        {
            _oldSliderVal = slider.value;
            _moveTiming = 1f;
            _sliderChgTime = Time.time;
        }
        if (Time.time > _sliderChgTime + _moveTiming)
        {
            if (_oldSlider.value > _oldSliderVal)
            {
                _oldSlider.value -= (_oldSlider.value - _oldSliderVal) * Time.deltaTime;
            }
        }
       
    }

    void FixedUpdate()
    {
        if (slider.value < 0.3f && slider.value > 0.0f)
        {
            var tmp = _bg.GetComponent<Image>().color;
            if (flashCnt / 10 % 2 == 0)
            {
                tmp.a = 0;
            }
            else
            {
                tmp.a = 1;
            }
            _bg.GetComponent<Image>().color = tmp;
        }
        flashCnt++;
    }
}


