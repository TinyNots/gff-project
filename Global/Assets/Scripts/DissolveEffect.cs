using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private float _dissolveAmount;
    private bool _isDissolving;

    private void Update()
    {
        if(_isDissolving)
        {
            _dissolveAmount = Mathf.Clamp01(_dissolveAmount + Time.deltaTime);
            material.SetFloat("_DissolveAmount", _dissolveAmount);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            _isDissolving = true;
        }
    }
}
