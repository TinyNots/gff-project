﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    private float tiltLen = 0;
    private float hitTime;
    private bool moveable = true;
    public float stunTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (moveable == false)
        {
            if (tiltLen >= 0)
            {
                switch (tiltLen % 2)
                {
                    case 0:
                        transform.position -= transform.TransformDirection(0.1f, 0f, 0f);
                        tiltLen--;

                        break;
                    case 1:
                        transform.position += transform.TransformDirection(0.1f, 0f, 0f);
                        tiltLen--;
                        break;
                }
            }
        }
        if (Time.time > hitTime + stunTime && tiltLen <=0)
        {
            
            moveable = true;
            return;
        }
    }

    public bool Moveable
    {
        get { return moveable; }
    }

    public void GetHitInit(float stunTimeSetting = 2)
    {
        hitTime = Time.time;
        if (tiltLen < 16)
        {
            tiltLen += 16;
        }
        stunTime = stunTimeSetting;
        moveable = false;
    }
}
