using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashPrefab;
    [SerializeField]
    private GameObject _meleePrefab;
    [SerializeField]
    private GameObject _dropPrefab;
    [SerializeField]
    private GameObject _dashPrefab;
    private Transform _shadow;
    private Animator _animator;
    private int _comboCount;

    // Temporary
    private Character _character;

    // Debug
    [SerializeField]
    private float _delayTime = 0.5f;

    private void Start()
    {
        _shadow = transform.parent.Find("Shadow");
        _animator = transform.GetComponent<Animator>();
        _comboCount = 0;
        _character = transform.parent.GetComponent<Character>();
    }

    public void Attack()
    {
        GameObject tmpHitBox = Instantiate(_slashPrefab, transform);
        tmpHitBox.transform.GetComponent<Depth>().DepthSetting = _shadow.position.y;
        tmpHitBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(tmpHitBox, 0.2f);
    }

    public void AttackWithShake()
    {
        CameraShaker.ShakeOnce(0.5f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 1.0f);
        BetterPlayerControl playerControl = transform.parent.GetComponent<BetterPlayerControl>();
        playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.65f, 0.65f));
        Attack();
    }

    public void AttackNextHit()
    {
        GameObject slashBox = Instantiate(_meleePrefab, transform);
        slashBox.GetComponent<Damage>().EnableNextHit();
        slashBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(slashBox, 0.2f);
    }

    public void IncreaseCombo()
    {
        _comboCount++;
        _animator.SetInteger("ComboCount", _comboCount);
        SoundManager.Instance.PlaySe("Whoosh 4_" + Random.Range(1, 5));
    }

    public void ResetCombo()
    {
        _comboCount = 0;
        _animator.SetInteger("ComboCount", _comboCount);
        _animator.SetBool("Attack", false);
        _animator.ResetTrigger("Hit");
    }

    public void Melee()
    {
        GameObject hitBox = Instantiate(_meleePrefab, transform);
        hitBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(hitBox, 0.2f);
    }

    public void MeleeNextHit()
    {
        GameObject hitBox = Instantiate(_meleePrefab, transform);
        hitBox.GetComponent<Damage>().EnableNextHit();
        hitBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(hitBox, 0.2f);
    }

    public void SpawnDrop()
    {
        GameObject hitBox = Instantiate(_dropPrefab, transform);
        hitBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(hitBox, 0.2f);
    }

    public void PlaySound(string name)
    {
        SoundManager.Instance.PlaySe(name + Random.Range(1, 5));
    }

    public void PlayNormalSound(string name)
    {
        SoundManager.Instance.PlaySe(name);
    }

    public void DashBox()
    {
        GameObject hitBox = Instantiate(_dashPrefab, transform);
        hitBox.GetComponent<Damage>().SetOwner(transform);
        Destroy(hitBox, 0.2f);
    }

    public void DelayAttack()
    {
        StartCoroutine(Wait(_delayTime));
    }

    private IEnumerator Wait(float time)
    {
        _character.EnableAttack = false;
        yield return new WaitForSeconds(time);
        _character.EnableAttack = true;
    }

    public void PlayRunSound()
    {
        SoundManager.Instance.PlaySe("Light Armor Gravel Running 1_0" + Random.Range(1, 9));
    }
}
