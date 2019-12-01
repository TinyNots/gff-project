using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    static private float hp = 100;
    [SerializeField]
    private GameObject dmgImage;
    private GameObject prefab = null; 
    private bool receiveDmgFlag = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ReceiveDmgEffect();
    }
    public void ReceiveDmg(float value)
    {
        hp -= value;
        Debug.Log(value + "Damage");
        receiveDmgFlag = true;
    }

    public void ReceiveDmgEffect()
    {

        if (!receiveDmgFlag)
        {
            return;
        };

        prefab = Instantiate(dmgImage, transform.position, transform.rotation) as GameObject;
        GameObject canvas = GameObject.Find("Canvas");
        prefab.transform.SetParent(canvas.transform);
        prefab.SetActive(true);
        prefab.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        receiveDmgFlag = false;


    }
}
