using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripple : MonoBehaviour
{
    public Transform pointPrefab;

    [Range(10, 100)]
    public int resolution = 10;

    Transform[] points;

    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[resolution * resolution];
        for (int i = 0, z = 0; z < resolution; z++)
        {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                Transform point = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                point.localPosition = position;
                points[i] = point;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            float t = Time.time;
            float d = Mathf.Sqrt(position.x * position.x +position.z * position.z);
            float y = Mathf.Sin(Mathf.PI * (4f * d - t));
            y /= 1f + 10f * d;
            position.y = y;
            point.localPosition = position;
            point.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
        }
    }
}
