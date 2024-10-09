using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    // Cambiamos 'private' a 'public' para poder asignarlo desde el inspector
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Si 'player' no se asigna desde el inspector, asignamos un agente por defecto
        if (player == null)
        {
            player = FindObjectOfType<Agent>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}