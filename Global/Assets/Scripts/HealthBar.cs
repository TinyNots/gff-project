using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float maxhp;
    GameObject health;
    public int playerCnt;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        health = gameObject.Find
        maxhp = health.HP;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.hp / maxhp;

    }


}
