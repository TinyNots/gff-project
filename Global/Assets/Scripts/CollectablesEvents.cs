using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesEvents : MonoBehaviour
{
    public void ResetVelocity()
    {
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }
}
