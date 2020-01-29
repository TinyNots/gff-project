using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;

    private Transform _shadow;

    // Start is called before the first frame update
    void Start()
    {
        _shadow = transform.parent.Find("Shadow");
    }

    public void Attack()
    {
        GameObject slash = Instantiate(_slashPrefab, transform);
        slash.GetComponent<Depth>().DepthSetting = _shadow.position.y;
        slash.GetComponent<Damage>().SetOwner(transform);
    }
}
