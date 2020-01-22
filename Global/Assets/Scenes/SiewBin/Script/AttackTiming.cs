using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTiming : MonoBehaviour
{
    [SerializeField]
    private GameObject attackBox;   //近攻撃の範囲か遠攻撃の弾(プロトタイプ)
    private GameObject tmpSlash;    //プロトタイプを複製
    private int maxNum = 3;
    float frame = 0;
    bool waitFlag = false;
    int idx = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (frame % 10 == 0)
        {
            SpawnAttack();
            Quaternion target = Quaternion.AngleAxis((20 * (idx - (maxNum / 2))), new Vector3(0, 0, 1));
            tmpSlash = Instantiate(AttackBox, transform.position, target * transform.rotation);
            //// tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);

            tmpSlash.GetComponent<Damage>().SetOwner(transform);
            //tmpSlash.transform.Find("Shadow").transform.rotation = Quaternion.Euler(tmpSlash.transform.rotation.x, tmpSlash.transform.rotation.y,- tmpSlash.transform.rotation.z);
            tmpSlash.SetActive(true);
            idx++;
            idx %= 18;

            //-------------
            //tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);


        }
    }

    public GameObject AttackBox
    {
        get { return attackBox; }
    }

    //攻撃する
    public void SpawnAttack()
    {
        for (int i = 0; i < 10; i++)
        {
            Quaternion target = Quaternion.AngleAxis((36 * (i - (maxNum / 2))), new Vector3(0, 0, 1));
            tmpSlash = Instantiate(AttackBox, transform.position, target * transform.rotation);
            // tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
            tmpSlash.GetComponent<Projectile>().MultiShotIdx = i + 1;

            tmpSlash.GetComponent<Damage>().SetOwner(transform);
            //tmpSlash.transform.Find("Shadow").transform.rotation = Quaternion.Euler(tmpSlash.transform.rotation.x, tmpSlash.transform.rotation.y,- tmpSlash.transform.rotation.z);
            tmpSlash.SetActive(true);
        }


        //tmpSlash = Instantiate(AttackBox, transform.position,transform.rotation);
        //tmpSlash.GetComponent<Damage>().SetOwner(transform);
        //tmpSlash.SetActive(true);
    }

    //攻撃終わる
    public void ResetAttack()
    {
        if (tmpSlash != null)
        {
            Destroy(tmpSlash);
        }
    }

    public void SpawnMultipleProjectile()
    {
        for (int i = 0; i < maxNum; i++)
        {
            //Quaternion target = Quaternion.AngleAxis((15 * (i - (maxNum / 2))), new Vector3(0,0,1));
            //tmpSlash = Instantiate(AttackBox, transform.position, target * transform.rotation);
            tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
            tmpSlash.GetComponent<Projectile>().MultiShotIdx = i + 1;

            tmpSlash.GetComponent<Damage>().SetOwner(transform);
            //tmpSlash.transform.Find("Shadow").transform.rotation = Quaternion.Euler(tmpSlash.transform.rotation.x, tmpSlash.transform.rotation.y,- tmpSlash.transform.rotation.z);
            tmpSlash.SetActive(true);
        }
    }

}
