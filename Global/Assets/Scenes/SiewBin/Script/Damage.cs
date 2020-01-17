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

    private bool isDamaged = false;
    //[TagSelector]
    private string[] targetTag = new string[] { "Enemy", "Player" };
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Transform _hitPrefab;

    // Debug
    private bool waiting = false;

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
        _shadow = transform.parent.parent.Find("Shadow");
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
            float depthB = collision.gameObject.transform.parent.Find("Shadow").GetComponent<Depth>().DepthSetting;

            if (depthB <= _depth + _shadowSize.y / 2.0f && depthB >= _depth - _shadowSize.y / 2.0f)
            {
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
                    }

                    if(collision.gameObject.tag == targetTag[(int)SelectableTag.Player])
                    {
                        BetterPlayerControl playerControl = collision.transform.parent.GetComponent<BetterPlayerControl>();
                        playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.5f, 0.5f));
                    }

                    // Debug
                    //FindObjectOfType<HitStop>().Stop(0.02f);

                    if(_hitStopTime != 0.0f)
                    {
                        Animator ownerAnimator = transform.parent.GetComponent<Animator>();
                        Animator otherAnimator = collision.transform.GetComponent<Animator>();
                        FindObjectOfType<AnimationStopper>().StopAnimation(ownerAnimator, _hitStopTime);
                        FindObjectOfType<AnimationStopper>().StopAnimation(otherAnimator, _hitStopTime);
                    }

                    if(_hitPrefab != null)
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
}
