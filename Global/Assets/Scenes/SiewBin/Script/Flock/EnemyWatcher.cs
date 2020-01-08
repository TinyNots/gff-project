using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcher : MonoBehaviour
{
    public Transform boidController;

    void LateUpdate()
    {
        if (boidController)
        {
            Vector3 watchPoint = boidController.GetComponent<EnemyController>().flockCenter;
            transform.LookAt(watchPoint + boidController.transform.position);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
