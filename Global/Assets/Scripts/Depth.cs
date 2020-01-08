using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    [SerializeField]
    private float _depth = 0.0f;

    public float DepthSetting
    {
        get { return _depth; }
        set { _depth = value; }
    }
}
