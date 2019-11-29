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
            dmgVel -= 10f * Time.deltaTime;
            transform.Translate(Vector2.up * dmgVel * Time.deltaTime);
        }
        if (dmgVel <= -1f)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

    }
}
