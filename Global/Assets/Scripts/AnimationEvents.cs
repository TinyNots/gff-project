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

    public void Attack()
    {
        GameObject tmpHitBox = Instantiate(_slashPrefab, transform);
        tmpHitBox.transform.GetComponent<Depth>().DepthSetting = _shadow.position.y;
        Destroy(tmpHitBox, 0.2f);
    }

    public void AttackWithShake()
    {
        CameraShaker.ShakeOnce(0.2f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 0.2f);
        BetterPlayerControl playerControl = transform.parent.GetComponent<BetterPlayerControl>();
        playerControl.RumbleController(0.35f, 0.0f, new Vector2(0.60f, 0.60f));
        Attack();
    }
}
