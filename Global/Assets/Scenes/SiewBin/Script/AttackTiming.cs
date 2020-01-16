using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTiming : MonoBehaviour
{
    [SerializeField]
    private GameObject attackBox;   //近攻撃の範囲か遠攻撃の弾(プロトタイプ)
    private GameObject tmpSlash;    //プロトタイプを複製
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject AttackBox
    {
        get { return attackBox; }
    }

    //攻撃する
    public void SpawnAttack()
    {
        tmpSlash = Instantiate(AttackBox, transform.position,transform.rotation);
        tmpSlash.GetComponent<Damage>().SetOwner(transform);
        tmpSlash.SetActive(true);
    }

    //攻撃終わる
    public void ResetAttack()
    {
        if (tmpSlash != null)
        {
            Destroy(tmpSlash);
        }
    }
}
