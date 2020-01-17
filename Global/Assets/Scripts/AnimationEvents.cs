using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;
    [SerializeField]
    private GameObject _meleePrefab;
    private Transform _shadow;
    private Animator _animator;
    private int _comboCount;

    private void Start()
    {
        _shadow = transform.parent.Find("Shadow");
        _animator = transform.GetComponent<Animator>();
        _comboCount = 0;
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
        playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.65f, 0.65f));
        Attack();
    }

    public void IncreaseCombo()
    {
        _comboCount++;
        _animator.SetInteger("ComboCount", _comboCount);
    }

    public void ResetCombo()
    {
        _comboCount = 0;
        _animator.SetInteger("ComboCount", _comboCount);
    }

    public void Melee()
    {
        GameObject hitBox = Instantiate(_meleePrefab, transform);
        Destroy(hitBox, 0.2f);
    }

    public void MeleeNextHit()
    {
        GameObject hitBox = Instantiate(_meleePrefab, transform);
        hitBox.GetComponent<Damage>().EnableNextHit();
        Destroy(hitBox, 0.2f);
    }
}
