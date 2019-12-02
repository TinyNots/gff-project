using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float _depth = 0f;
    private bool _isEableMove = true;

    [Header("Speed Setting")]
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    [Range(0, 1f)]
    private float _speedPercentZ = 0.5f;

    public void Update()
    {
        _depth = transform.position.y;
    }

    public float GetDepth()
    {
        return _depth;
    }

    public bool EableMove
    {
        get { return _isEableMove; }
        set { _isEableMove = value; }
    }
}