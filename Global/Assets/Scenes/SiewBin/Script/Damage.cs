using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int dmgVal = 1;
    private bool isDamaged = false;
    //[TagSelector]
    private string[] targetTag = new string[] { "Enemy", "Player" };
    public enum SelectableTag
    {
        Enemy,
        Player
    };
    public SelectableTag target = SelectableTag.Enemy;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //-------狼がバグ中(一時的直し)--------//
        if (gameObject.activeSelf)
        {
            gameObject.transform.position += new Vector3(0.00001f, 0f, 0f);
        }
        //-------狼がバグ中(一時的直し)--------//
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDamaged)
        {
            if (collision.gameObject.tag == targetTag[(int)target])
            {
                if (collision.gameObject.GetComponent<Health>().HP > 0)
                {
                    collision.gameObject.GetComponent<Health>().ReceiveDmg(dmgVal);
                    isDamaged = true;
                    Debug.Log(targetTag[(int)target] + " got hit");
                }
            }
        }
    }

    public int DmgVal
    {
        set { dmgVal = value; }
        get { return dmgVal; }
    }
}
