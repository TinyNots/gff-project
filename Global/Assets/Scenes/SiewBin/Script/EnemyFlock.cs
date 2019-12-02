using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlock : MonoBehaviour
{

    private Rigidbody2D rigid2D;
    private GameObject Controller;
    private bool inited = false;
    private float minVelocity;
    private float maxVelocity;
    private float randomness;
    private GameObject leader;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BoidSteering");
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BoidSteering()
    {
        while (true)
        {
            if (inited)
            {
                var calc2D = new Vector2(Calc().x, Calc().y);
                rigid2D.velocity = rigid2D.velocity + calc2D * Time.deltaTime;

                // enforce minimum and maximum speeds for the boids
                float speed = rigid2D.velocity.magnitude;
                if (speed > maxVelocity)
                {
                    rigid2D.velocity = rigid2D.velocity.normalized * maxVelocity;
                }
                else if (speed < minVelocity)
                {
                    rigid2D.velocity = rigid2D.velocity.normalized * minVelocity;
                }
            }

            float waitTime = Random.Range(0.3f, 0.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 Calc()
    {
        //-1 ~ 1
        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1,0);

        randomize.Normalize();
        EnemyController boidController = Controller.GetComponent<EnemyController>();
        Vector3 flockCenter = boidController.flockCenter;
        Vector2 flockVelocity = boidController.flockVelocity;
        Vector3 follow = leader.transform.localPosition;

        flockCenter = flockCenter - transform.localPosition;
        flockVelocity = flockVelocity - rigid2D.velocity;
        follow = follow - transform.localPosition;
        Vector3 tmpVec = new Vector3(flockVelocity.x, flockVelocity.y, 0);
        return (flockCenter + tmpVec + follow * 2 + randomize * randomness);
    }

    public void SetController(GameObject theController)
    {
        Controller = theController;
        EnemyController boidController = Controller.GetComponent<EnemyController>();
        minVelocity = boidController.minVelocity;
        maxVelocity = boidController.maxVelocity;
        randomness = boidController.randomness;
        leader = boidController.leader;
        inited = true;
    }
}
