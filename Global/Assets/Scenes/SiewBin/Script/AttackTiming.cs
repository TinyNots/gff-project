using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTiming : MonoBehaviour
{
    [SerializeField]
    private GameObject _attackBox;   //近攻撃の範囲か遠攻撃の弾(プロトタイプ)
    private GameObject _tmpSlash;    //プロトタイプを複製
    private int _maxNum = 3;
    float _frame = 0;
    bool _waitFlag = false;
    int _projIdx = 0;
    bool _delayFlag = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _frame++;
        if (_delayFlag)
        {
            if (_frame % 10 == 0)
            {
                Quaternion target = Quaternion.AngleAxis((20 * (_projIdx - (_maxNum / 2))), new Vector3(0, 0, 1));
                _tmpSlash = Instantiate(AttackBox, transform.position, target * transform.rotation);
                //// tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
                if (Random.Range(0, 2) == 0)
                {
                    _tmpSlash.GetComponent<Projectile>().SigmoidMove = true;
                }
                _tmpSlash.GetComponent<Damage>().SetOwner(transform);
                _tmpSlash.transform.Find("ShadowRotation").transform.rotation = Quaternion.Euler(_tmpSlash.transform.rotation.x, _tmpSlash.transform.rotation.y,- _tmpSlash.transform.rotation.z);
                _tmpSlash.SetActive(true);
                _projIdx++;
                if (_projIdx >= 18)
                {
                    _delayFlag = false;
                    _projIdx = 0;
                }
            }
        }
    }

    public GameObject AttackBox
    {
        get { return _attackBox; }
    }

    //攻撃する
    public void SpawnAttack()
    {


        _tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
        _tmpSlash.GetComponent<Damage>().SetOwner(transform);
        _tmpSlash.SetActive(true);
    }

    //攻撃終わる
    public void ResetAttack()
    {
        if (_tmpSlash != null)
        {
            Destroy(_tmpSlash);
        }
    }

    public void SpawnMultipleProjectile()
    {
        for (int i = 0; i < _maxNum; i++)
        {
            //Quaternion _target = Quaternion.AngleAxis((15 * (i - (maxNum / 2))), new Vector3(0,0,1));
            //tmpSlash = Instantiate(AttackBox, transform.position, _target * transform.rotation);
            _tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
            _tmpSlash.GetComponent<Projectile>().MultiShotIdx = i + 1;

            _tmpSlash.GetComponent<Damage>().SetOwner(transform);
           //tmpSlash.transform.Find("ShadowRotation").transform.rotation = Quaternion.Euler(tmpSlash.transform.rotation.x, tmpSlash.transform.rotation.y,- tmpSlash.transform.rotation.z);
            _tmpSlash.SetActive(true);
        }
    }

    public void BossSpawnProjectile()
    {
        if (Random.Range(0, 2) == 0)
        {

            for (int i = 0; i < 10; i++)
            {
                Quaternion target = Quaternion.AngleAxis((36 * (i - (_maxNum / 2))), new Vector3(0, 0, 1));
                _tmpSlash = Instantiate(AttackBox, transform.position, target * transform.rotation);
                // tmpSlash = Instantiate(AttackBox, transform.position, transform.rotation);
                if (Random.Range(0, 2) == 0)
                {
                    _tmpSlash.GetComponent<Projectile>().SigmoidMove = true;
                    if (Random.Range(0, 2) == 0)
                    {
                        _tmpSlash.GetComponent<Projectile>().ReverseSigmoid = true;
                    }
                }
                _tmpSlash.GetComponent<Damage>().SetOwner(transform);
                _tmpSlash.transform.Find("ShadowRotation").transform.rotation = Quaternion.Euler(_tmpSlash.transform.rotation.x, _tmpSlash.transform.rotation.y,- _tmpSlash.transform.rotation.z);
                _tmpSlash.SetActive(true);
            }
           
        }
        else
        {
            _delayFlag = true;
        }
    }
}
