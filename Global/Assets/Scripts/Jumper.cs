using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Jump Setting")]
    [SerializeField]
    private float _jumpVelocity = 10f;
    [SerializeField]
    private float _fallMultiplier = 3.0f;
    private float _currentVelocity = 0;
    private bool _isGrounded = true;
    private Character _character;

    [Header("Others")]
    [SerializeField]
    private Vector2 _offset = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        _character = gameObject.GetComponentInParent<Character>();
        _offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && _character.EableMove)
        {
            _isGrounded = false;
            _currentVelocity = _jumpVelocity;
        }

        if (!_isGrounded)
        {
            if (_currentVelocity <= 0)
            {
                _currentVelocity -= 10f * Time.deltaTime * _fallMultiplier;
            }
            else
            {
                _currentVelocity -= 10f * Time.deltaTime;
            }

            if (transform.localPosition.y < _offset.y)
            {
                transform.localPosition = _offset;
                _currentVelocity = 0;
                _isGrounded = true;
                StartCoroutine("JumpWait");
            }
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * _currentVelocity * Time.deltaTime);
    }

    private IEnumerator JumpWait()
    {
        _character.EableMove = false;

        yield return new WaitForSeconds(0.1f);

        _character.EableMove = true;
    }
}
