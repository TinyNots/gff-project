using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    private float _dmgVel;

    // Start is called before the first frame update
    void Start()
    {
        _dmgVel = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            _dmgVel += 30f * Time.deltaTime;
            this.gameObject.transform.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0);
            transform.Translate(Vector2.up * _dmgVel * Time.deltaTime);
        }
        //if (_dmgVel > 50f)
        //{
        //    this.gameObject.SetActive(false);
        //    Destroy(this.gameObject);
        //}
        if (GetComponent<Text>().color.a <= 0)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (GetComponent<Text>().color.a > 0)
        {
            var alpha = GetComponent<Text>().color;
            alpha.a -= 1f * Time.deltaTime;
            GetComponent<Text>().color = alpha;
        }
    }

}
