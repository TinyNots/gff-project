using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int dmgVal = 1;
    private bool isDamaged = false;
    //[TagSelector]
    private string[] targetTag = new string[] { "Enemy", "Player" };
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);

    [Header("Debug")]
    [SerializeField]
    private Transform _owner;

    public enum SelectableTag
    {
        Enemy,
        Player
    };
    public SelectableTag target = SelectableTag.Enemy;
    float _depth; 

    // Start is called before the first frame update
    void Start()
    {
        if (_owner)
        {
            _shadow = _owner.parent.Find("Shadow");
        }
        else
        {
            _shadow = transform.parent.parent.Find("Shadow");

        }
        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
        _depth = _shadow.GetComponent<Depth>().DepthSetting;
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
        if (collision.gameObject.tag == targetTag[(int)target])
        {
            float depthB = collision.gameObject.transform.parent.Find("Shadow").GetComponent<Depth>().DepthSetting;

            if (depthB <= _depth + _shadowSize.y / 2.0f && depthB >= _depth - _shadowSize.y / 2.0f)
            {
                if (collision.gameObject.GetComponent<Health>().HP > 0)
                {
                    collision.gameObject.GetComponent<Health>().ReceiveDmg(dmgVal);
                    Debug.Log(targetTag[(int)target] + " got hit");
                    FindObjectOfType<HitStop>().Stop(0.06f);
                    collision.transform.GetComponent<Flasher>().StartFlash();
                    Character character = collision.transform.parent.transform.GetComponent<Character>();
                    character.IsHurt = true;
                }
            }

        }
    }

    public int DmgVal
    {
        set { dmgVal = value; }
        get { return dmgVal; }
    }

    public void SetOwner(Transform owner)
    {
        _owner = owner;
    }
}
