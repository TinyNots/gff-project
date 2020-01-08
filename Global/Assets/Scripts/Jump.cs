using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private float _jumpVelocity = 10f;
    private float _currentVelocity = 0;
    private bool _isGrounded = true;
    [SerializeField]
    private PlayerMovement player;
    private int _controllerIndex;
    [SerializeField]
    private GameObject _slash;
    private GameObject _tmpSlash;
    [SerializeField]
    private float _fallMultiplier = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerMovement>();
        _controllerIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_controllerIndex == 0)
        {
            _controllerIndex = player.GetControllerIndex();
            return;
        }

        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * -100);

        if (GamePadManager.Instance.GetGamepad(_controllerIndex).GetButtonDown("A") && _isGrounded == true)
        {
            _currentVelocity = _jumpVelocity;
            _isGrounded = false;
            GetComponent<Animator>().SetBool("IsJumping", true);
        }

        if(!_isGrounded)
        {
            
            if(_currentVelocity <= 0)
            {
                _currentVelocity -= 10f * Time.deltaTime * _fallMultiplier;
            }
            else
            {
                _currentVelocity -= 10f * Time.deltaTime;
            }
            transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);

            GetComponent<Animator>().SetFloat("JumpVelocity", _currentVelocity);

            if (transform.localPosition.y < 0)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                _currentVelocity = 0;
                _isGrounded = true;
                StartCoroutine("JumpWait");
                GetComponent<Animator>().SetBool("IsJumping", false);
            }
        }

        if (GamePadManager.Instance.GetGamepad(_controllerIndex).GetStickL().X < -0.01f)
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (GamePadManager.Instance.GetGamepad(_controllerIndex).GetStickL().X > 0.01f)
        {
            transform.localRotation = Quaternion.Euler(0f, 0, 0f);
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
        Destroy(_tmpSlash);
        player.EableMove = true;
    }

    public void ResetHurt()
    {
        GetComponent<Animator>().SetBool("IsHurt", false);
        player.EableMove = true;
    }

    public void SpawnSlash()
    {
        _tmpSlash = Instantiate(_slash, transform);
    }
}
