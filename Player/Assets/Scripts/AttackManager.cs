using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private float _waitTime = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator == null)
        {
            Debug.LogError("Animator is missing.");
            return;
        }

        if (_animator.GetBool("Moveable"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _animator.SetTrigger("Attack");
            }
        }
    }

    public IEnumerator StopInput()
    {
        _animator.SetBool("Moveable", false);
        yield return new WaitForSeconds(_waitTime);
        _animator.SetBool("Moveable", true);
    }
}
