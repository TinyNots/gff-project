using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private float _jumpVelocity = 30f;
    private float _currentVelocity = 0;
    private bool _isGrounded = true;
    [SerializeField]
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GamePadManager.Instance.GetGamepad(1).GetButtonDown("A") && _isGrounded == true)
        {
            _currentVelocity = _jumpVelocity;
            _isGrounded = false;
            GetComponent<Animator>().SetBool("IsJumping", true);
        }

        if(!_isGrounded)
        {
            _currentVelocity -= 5f * 0.2f;
            transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);

            GetComponent<Animator>().SetFloat("JumpVelocity", _currentVelocity);

            if (_currentVelocity <= -_jumpVelocity)
            {
                transform.localPosition = new Vector2(0, 0);
                _currentVelocity = 0;
                _isGrounded = true;
                StartCoroutine("JumpWait");
                GetComponent<Animator>().SetBool("IsJumping", false);
            }
        }
    }

    private IEnumerator JumpWait()
    {
        player.EableMove = false;

        yield return new WaitForSeconds(0.1f);

        player.EableMove = true;
    }

    public void ResetAttack()
    {
        GetComponent<Animator>().SetBool("IsAttacking", false);
        player.EableMove = true;
    }
}
