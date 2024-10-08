using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyWandering : MonoBehaviour 
{
    public NavMeshAgent agent;
    public Transform centrePoint;
    public Transform player;
    public float range;

    public float detectionRadius = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distancePlayer = Vector3.Distance(player.position, transform.position);

        if (distancePlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 punto;
            if (TryGetRandomPoint(centrePoint.position, range, out punto)) 
            {
                agent.SetDestination(punto);
            }
        }

    }
    bool TryGetRandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        // Generar un punto aleatorio dentro de la esfera alrededor del centro
        Vector3 potentialPoint = center + Random.insideUnitSphere * range;

        // Realizar la búsqueda en el NavMesh
        if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            point = navHit.position;
            return true;
        }

        // Si no se encuentra un punto válido en el NavMesh, devolver cero
        point = Vector3.zero;
        return false;
    }

    void FollowPlayer()
    {
        agent.destination = player.position;
    }


}