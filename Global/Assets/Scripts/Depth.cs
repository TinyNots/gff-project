using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    [SerializeField]
    private float _depth = 0.0f;
    private bool _updateFlag = true;

    private void Update()
    {
       if(!_updateFlag)
        {
            return;
        }

        _depth = transform.position.y;
    }

    public float DepthSetting
    {
        get { return _depth; }
        set
        {
            _depth = value;
            _updateFlag = false;
        }
    }
}