using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public float _hp = 100;
    [SerializeField]
    private GameObject _dmgImage;
    private GameObject _prefab = null;

    private bool _receiveDmgFlag = false;
    private GameObject _dmgOrigin = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ReceiveDmgEffect();
    }

    private void FixedUpdate()
    {
       
    }
    public void ReceiveDmg(float value,GameObject dmgOrigin)
    {
        _hp -= value;
        Debug.Log(value + "Damage");
        _dmgImage.GetComponent<Text>().text = value.ToString();
        _receiveDmgFlag = true;
        _dmgOrigin = dmgOrigin;
    }

    public void Repair()
    {
        Resurrection canvas = GameObject.Find("RecoverySlider" + i).GetComponent<Resurrection>();
    }

    private void LateUpdate()
    {
        _receiveDmgFlag = false;

    }

    public void ReceiveDmgEffect()
    {

        if (!_receiveDmgFlag)
        {
            return;
        };

        _prefab = Instantiate(_dmgImage, transform.position, transform.rotation) as GameObject;
        GameObject canvas = GameObject.Find("Canvas");

        _prefab.transform.SetParent(canvas.transform);
        _prefab.SetActive(true);
        var offset = Random.Range(-gameObject.GetComponent<Collider2D>().bounds.size.x / 2, gameObject.GetComponent<Collider2D>().bounds.size.x/2);
        _prefab.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x + offset, this.transform.position.y + 1f, this.transform.position.z));
        _prefab.transform.rotation = Quaternion.Euler(new Vector3(0, 0));

    }

    public float HP
    {
        get { return _hp; }
    }
    public bool ReceiveDmgFlag
    {
        get { return _receiveDmgFlag; }
    }

    public GameObject DmgOrigin
    {
        get { return _dmgOrigin; }

    }
}
