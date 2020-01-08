using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;

    public void Attack  ()
    {
        GameObject tmpHitBox = Instantiate(_slashPrefab, transform);
        tmpHitBox.transform.GetComponent<Depth>().DepthSetting = transform.position.y - transform.localPosition.y;
        Destroy(tmpHitBox, 0.2f);
    }
}
