using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    private float dmgVel;

    // Start is called before the first frame update
    void Start()
    {
       dmgVel = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            dmgVel += 30f * Time.deltaTime;
            this.gameObject.transform.localScale += new Vector3(1f * Time.deltaTime,1f * Time.deltaTime,0);
            transform.Translate(Vector2.up * dmgVel * Time.deltaTime);
        }
        if (dmgVel > 50f)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

    }
}
