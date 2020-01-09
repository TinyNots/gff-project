using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;
    private Transform _shadow;

    private void Start()
    {
        _shadow = transform.parent.Find("Shadow");
    }

    public void Attack  ()
    {
        GameObject tmpHitBox = Instantiate(_slashPrefab, transform);
        tmpHitBox.transform.GetComponent<Depth>().DepthSetting = _shadow.position.y;
        Destroy(tmpHitBox, 0.2f);
    }
}
