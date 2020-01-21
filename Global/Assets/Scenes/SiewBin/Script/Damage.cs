using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float _hitStopTime = 0.05f;
    [SerializeField]
    private int _damageValue = 1;

    //[TagSelector]
    private string[] targetTag = new string[] { "Enemy", "Player" };
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Transform _hitPrefab;
    private bool _nextHit = false;

    [SerializeField]
    private bool _isRange = false;

    public enum SelectableTag
    {
        Enemy,
        Player
    };
    public SelectableTag target = SelectableTag.Enemy;
    float _depth; 

    // Start is called before the first frame update
    void Start()
    {
        if(!_isRange)
        {
            _shadow = transform.parent.parent.Find("Shadow");
        }
        else
        {
            _shadow = transform.Find("Shadow");
        }

        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
        _depth = _shadow.GetComponent<Depth>().DepthSetting;
    }

    // Update is called once per frame
    void Update()
    {
        //-------狼がバグ中(一時的直し)--------//
        if (gameObject.activeSelf)
        {
            gameObject.transform.position += new Vector3(0.00001f, 0f, 0f);
        }
        //-------狼がバグ中(一時的直し)--------//
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetTag[(int)target])
        {
            Transform collisionShadow = collision.transform.parent.Find("Shadow");

            float collisionDepth = collisionShadow.GetComponent<Depth>().DepthSetting;
            Vector2 collisionShadowSize = collisionShadow.GetComponent<Renderer>().bounds.size;

            // Debug
            //Debug.DrawLine(
            //    new Vector2(collision.transform.position.x, collisionDepth) - collisionShadowSize / 2.0f,
            //    new Vector2(collision.transform.position.x, collisionDepth) + collisionShadowSize / 2.0f,
            //    Color.blue);

            //Debug.DrawLine(
            //    new Vector2(transform.position.x, _depth) - _shadowSize / 2.0f,
            //    new Vector2(transform.position.x, _depth) + _shadowSize / 2.0f,
            //    Color.green);

            Rect shadowA = new Rect((Vector2)_shadow.position - _shadowSize / 2.0f, _shadowSize);
            Rect shadowB = new Rect((Vector2)collisionShadow.position - collisionShadowSize / 2.0f, collisionShadowSize);

            Debug.DrawLine(shadowA.min, shadowB.min, Color.green);
            Debug.DrawLine(shadowB.min, shadowB.max, Color.blue);

            //Rect shadowA = new Rect(0.0f, 0.0f, _shadowSize.x, _shadowSize.y);
            //Rect shadowB = new Rect(0.0f, 0.0f, collisionShadowSize.x, collisionShadowSize.y);

            //shadowA.Overlaps(shadowB,true)

            if (shadowA.Overlaps(shadowB, true))
            {
                if(transform.CompareTag("Punch"))
                {
                    SoundManager.Instance.PlaySe("Punch 1_" + Random.Range(1, 5));
                }
                else if (transform.CompareTag("Slash"))
                {
                    SoundManager.Instance.PlaySe("Stab 8_" + Random.Range(1, 5));
                }

                if (collision.gameObject.GetComponent<Health>().HP > 0)
                {
                    collision.gameObject.GetComponent<Health>().ReceiveDmg(_damageValue);
                    Debug.Log(targetTag[(int)target] + " got hit");

                    if (collision.gameObject.tag == targetTag[(int)SelectableTag.Enemy])
                    {
                        // rumble the controller
                        BetterPlayerControl playerControl = transform.parent.parent.GetComponent<BetterPlayerControl>();
                        playerControl.RumbleController(0.1f, 0.0f, new Vector2(0.5f, 0.5f));

                        CameraShaker.ShakeOnce(0.05f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 0.5f);

                        // trigger the hit
                        if (_nextHit)
                        {
                            Animator playerAnimator = transform.parent.GetComponent<Animator>();
                            playerAnimator.SetTrigger("Hit");
                        }
                    }

                    if (collision.gameObject.tag == targetTag[(int)SelectableTag.Player])
                    {
                        BetterPlayerControl playerControl = collision.transform.parent.GetComponent<BetterPlayerControl>();
                        playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.5f, 0.5f));
                    }

                    // Debug
                    //FindObjectOfType<HitStop>().Stop(0.02f);

                    if (_hitStopTime != 0.0f)
                    {
                        Animator ownerAnimator = transform.parent.GetComponent<Animator>();
                        Animator otherAnimator = collision.transform.GetComponent<Animator>();
                        FindObjectOfType<AnimationStopper>().StopAnimation(ownerAnimator, _hitStopTime);
                        FindObjectOfType<AnimationStopper>().StopAnimation(otherAnimator, _hitStopTime);
                    }

                    if (_hitPrefab != null)
                    {
                        Instantiate(_hitPrefab, collision.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Debug.LogError("Hit Prefab is missing");
                    }

                    collision.transform.GetComponent<Flasher>().StartFlash(0.05f);
                    Character character = collision.transform.parent.transform.GetComponent<Character>();
                    character.IsHurt = true;
                }

            //if (collisionDepth <= _depth + _shadowSize.y / 2.0f && collisionDepth >= _depth - _shadowSize.y / 2.0f)
            //{
            //    if (collision.gameObject.GetComponent<Health>().HP > 0)
            //    {
            //        collision.gameObject.GetComponent<Health>().ReceiveDmg(_damageValue);
            //        Debug.Log(targetTag[(int)target] + " got hit");

            //        if (collision.gameObject.tag == targetTag[(int)SelectableTag.Enemy])
            //        {
            //            // rumble the controller
            //            BetterPlayerControl playerControl = transform.parent.parent.GetComponent<BetterPlayerControl>();
            //            playerControl.RumbleController(0.1f, 0.0f, new Vector2(0.5f, 0.5f));

            //            CameraShaker.ShakeOnce(0.05f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 0.5f);

            //            // trigger the hit
            //            if(_nextHit)
            //            {
            //                Animator playerAnimator = transform.parent.GetComponent<Animator>();
            //                playerAnimator.SetTrigger("Hit");
            //            }
            //        }

            //        if(collision.gameObject.tag == targetTag[(int)SelectableTag.Player])
            //        {
            //            BetterPlayerControl playerControl = collision.transform.parent.GetComponent<BetterPlayerControl>();
            //            playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.5f, 0.5f));
            //        }

            //        // Debug
            //        //FindObjectOfType<HitStop>().Stop(0.02f);

            //        if(_hitStopTime != 0.0f)
            //        {
            //            Animator ownerAnimator = transform.parent.GetComponent<Animator>();
            //            Animator otherAnimator = collision.transform.GetComponent<Animator>();
            //            FindObjectOfType<AnimationStopper>().StopAnimation(ownerAnimator, _hitStopTime);
            //            FindObjectOfType<AnimationStopper>().StopAnimation(otherAnimator, _hitStopTime);
            //        }

            //        if(_hitPrefab != null)
            //        {
            //            Instantiate(_hitPrefab, collision.transform.position, Quaternion.identity);
            //        }
            //        else
            //        {
            //            Debug.LogError("Hit Prefab is missing");
            //        }

            //        collision.transform.GetComponent<Flasher>().StartFlash(0.05f);
            //        Character character = collision.transform.parent.transform.GetComponent<Character>();
            //        character.IsHurt = true;
            //    }
            }

        }
    }

    public int DamageValue
    {
        set { _damageValue = value; }
        get { return _damageValue; }
    }

    private IEnumerator Wait(Animator animator,float duration)
    {
        animator.enabled = false;
        Debug.Log("Stop");
        yield return new WaitForSeconds(duration);
        animator.enabled = true;
        Debug.Log("Resume");
    }

    public void EnableNextHit()
    {
        _nextHit = true;
    }
}
