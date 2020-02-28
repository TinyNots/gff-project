using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;

    private Transform _shadow;

    // 初期化
    void Start()
    {
        _shadow = transform.parent.Find("Shadow");
    }

    // 当たり判定のボックスを生成する
    public void Attack()
    {
        GameObject slash = Instantiate(_slashPrefab, transform);
        slash.GetComponent<Depth>().DepthSetting = _shadow.position.y;
        slash.GetComponent<Damage>().SetOwner(transform);
    }
}
