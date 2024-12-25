using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex;

    void Start()
    {
        // Asegúrate de que haya al menos dos waypoints
        if (waypoints.Length < 2)
        {
            Debug.LogError("Se necesitan al menos 2 waypoints para patrullar.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();

        // Seleccionar aleatoriamente el punto de inicio
        currentWaypointIndex = Random.Range(0, waypoints.Length);

        // Establecer el destino inicial
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        // Si el agente ha llegado al waypoint actual
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Actualizar el waypoint para avanzar hacia adelante
            UpdateWaypoint();

            // Establecer la nueva posición de destino
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Actualizar el índice del waypoint para avanzar hacia adelante
    void UpdateWaypoint()
    {
        // Avanzar al siguiente waypoint
        currentWaypointIndex++;

        // Si llegamos al final de los waypoints, reiniciar al primero
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
    }
}
