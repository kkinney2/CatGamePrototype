using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private Vector3 homePosition;
    private float speed;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        homePosition = transform.position;
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (TimeManager.instance.DecimalTime >= 0.25f && TimeManager.instance.DecimalTime <= 0.75f)
        {
            if (timer >= wanderTimer)
            {
                agent.speed = speed;
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, homePosition) > 2f && agent.destination != homePosition)
            {
                agent.speed = speed * 2;
                agent.SetDestination(homePosition);
            }
        }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
