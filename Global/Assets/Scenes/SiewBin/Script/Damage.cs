using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int dmgVal = 1;
    private bool isDamaged = false;
    //[TagSelector]
    private string[] targetTag = new string[] { "Enemy", "Player" };
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);
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
                    collision.gameObject.GetComponent<Health>().ReceiveDmg(dmgVal);
                    Debug.Log(targetTag[(int)target] + " got hit");

                    if (collision.gameObject.tag == targetTag[(int)SelectableTag.Enemy])
                    {
                        // rumble the controller
                        //BetterPlayerControl playerControl = transform.parent.parent.GetComponent<BetterPlayerControl>();
                        //playerControl.RumbleController(0.1f, 0.0f, new Vector2(0.5f, 0.5f));

                        CameraShaker.ShakeOnce(0.05f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 0.5f);
                    }

                    if(collision.gameObject.tag == targetTag[(int)SelectableTag.Player])
                    {
                        BetterPlayerControl playerControl = collision.transform.parent.GetComponent<BetterPlayerControl>();
                        playerControl.RumbleController(0.2f, 0.0f, new Vector2(0.5f, 0.5f));
                    }

                    Animator playerAnimator = transform.parent.GetComponent<Animator>();
                    Animator enemyAnimator = collision.transform.GetComponent<Animator>();
                    StartCoroutine(Wait(playerAnimator, 0.05f));
                    StartCoroutine(Wait(playerAnimator, 0.05f));

                    collision.transform.GetComponent<Flasher>().StartFlash(0.05f);
                    Character character = collision.transform.parent.transform.GetComponent<Character>();
                    character.IsHurt = true;
                }
            }

        }
    }

    public int DmgVal
    {
        set { dmgVal = value; }
        get { return dmgVal; }
    }

    private IEnumerator Wait(Animator animator,float duration)
    {
        animator.enabled = false;
        yield return new WaitForSecondsRealtime(duration);
        animator.enabled = true;
    }
}
