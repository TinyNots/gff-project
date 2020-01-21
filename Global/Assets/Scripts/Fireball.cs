using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    bool rotaFlag = false;
  
    [SerializeField]
    private ParticleSystem particle;
    private float particleStartTime;
    private int rotaCnt;
    private bool startParticleFlag;
    [SerializeField]
    private ParticleSystem attParticle;
    private float attParticleStartTime;

    // Start is called before the first frame update
    void Start()
    {
        rotaCnt = 0;
        particleStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.x < -1.5f)
        {
            rotaFlag = true;

        }
        else if (transform.localPosition.x > 1.5f)
        {
            rotaFlag = false;
            if (!startParticleFlag)
            {
                rotaCnt++;
            }
        }
        if (rotaCnt % 3 < 2)
        {
            if (rotaFlag)
            {
                transform.localPosition += transform.TransformDirection(2.0f, -0.3f, 0.0f) * Time.deltaTime;
                gameObject.GetComponent<Renderer>().sortingOrder = transform.parent.Find("Sprite").GetComponent<Renderer>().sortingOrder + 1;
            }
            else
            {
                transform.localPosition += transform.TransformDirection(-2.0f, 0.3f, 0.0f) * Time.deltaTime;
                gameObject.GetComponent<Renderer>().sortingOrder = -1;

            }
        }
        else
        {
            if (!startParticleFlag)
            {
                particleStartTime = Time.time;
            }
            startParticleFlag = true;
            
        }
        if (startParticleFlag)
        {
            particle.gameObject.SetActive(true);
            if(particleStartTime + 5 < Time.time )
            {
                particle.Stop();
                //startParticleFlag = false;
                attParticle.gameObject.SetActive(true);
            }
            if (particleStartTime + 8 < Time.time)
            {
                attParticle.Stop();
                startParticleFlag = false;
            }
        }
    }
}
