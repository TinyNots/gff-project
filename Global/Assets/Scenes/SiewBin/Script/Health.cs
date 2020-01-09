using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public float hp = 100;
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
        dmgImage.GetComponent<Text>().text = value.ToString();
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
        prefab.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y +1f, this.transform.position.z));
        prefab.transform.rotation = Quaternion.Euler(new Vector3(0, 0));
        receiveDmgFlag = false;


    }

    public float HP
    {
        get { return hp; }
    }

    public bool ReceiveDmgFlag
    {
        get { return receiveDmgFlag; }
    }
}
