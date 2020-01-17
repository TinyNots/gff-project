using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStopper : MonoBehaviour
{
    public void StopAnimation(Animator animator,float duration)
    {
        StartCoroutine(Wait(animator, duration));
    }

    private IEnumerator Wait(Animator animator,float duration)
    {
        animator.enabled = false;
        yield return new WaitForSeconds(duration);
        animator.enabled = true;
    }
}
