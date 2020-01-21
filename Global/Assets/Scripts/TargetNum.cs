using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNum : MonoBehaviour
{
    private int targettedNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int TargettedNum
    {
        get { return targettedNum; }
        set { targettedNum = value; }
    }
}
