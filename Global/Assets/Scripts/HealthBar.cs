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
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
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
 
        slider.value = health._hp / maxhp;

    }


}
