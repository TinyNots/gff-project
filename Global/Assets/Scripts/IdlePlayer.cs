using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayer : MonoBehaviour
{
	private Animator _anim;
	[SerializeField]
	private bool fast = false;
    // Start is called before the first frame update
    void Start()
    {
		_anim = GetComponent<Animator>();
		if (_anim == null)
		{
			return;
		}
		_anim.SetBool("sword", fast);
	}

	public void Sword()
	{
		if(_anim == null)
		{
			return;
		}
		_anim.SetBool("sword", true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ResetEableMove()
	{

	}
}
