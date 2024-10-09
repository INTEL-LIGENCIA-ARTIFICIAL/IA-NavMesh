using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingAgent : MonoBehaviour
{
    public NavMeshAgent agent;               // NavMesh agent for pathfinding
    public Transform[] waypoints;            // Array of waypoints for patrolling
    public GameObject ghostAgent;            // The ghost agent to follow
    public float followDistance = 10f;       // Distance at which the agent follows the ghost
    public float patrolSpeed = 3.5f;         // Speed while patrolling
    public float followSpeed = 5f;           // Speed while following the ghost

    private int currentWaypointIndex;        // Current waypoint index
    private bool isForward;                  // Direction of movement (true = forward)
    private bool isFollowingGhost = false;   // Is agent following the ghost?

    void Start()
    {
        // Ensure we have at least two waypoints
        if (waypoints.Length < 2)
        {
            Debug.LogError("Se necesitan al menos 2 waypoints para patrullar.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();

        // Select a random starting point and direction
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        isForward = Random.value > 0.5f;  // Randomly decide to go forward or backward

        // Set the agent's initial speed to patrol speed
        agent.speed = patrolSpeed;

        // Move to the initial waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Disable the ghost's MeshRenderer (make it invisible)
        ghostAgent.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        // Calculate distance to the ghost
        float distanceToGhost = Vector3.Distance(transform.position, ghostAgent.transform.position);

        // If the ghost is close enough, follow the ghost
        if (distanceToGhost < followDistance)
        {
            isFollowingGhost = true;
            agent.speed = followSpeed;  // Increase speed when following the ghost
            agent.SetDestination(ghostAgent.transform.position);
        }
        else
        {
            // If the ghost is too far, stop following and go back to patrolling
            isFollowingGhost = false;
            agent.speed = patrolSpeed;

            // If the agent has reached its current waypoint, move to the next one
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                UpdateWaypoint();
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    // Update the waypoint index based on the current direction
    void UpdateWaypoint()
    {
        if (isForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // If we reach the end, reverse direction
                currentWaypointIndex = waypoints.Length - 1;
                isForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                // If we reach the start, reverse direction
                currentWaypointIndex = 0;
                isForward = true;
            }
        }
    }
}
