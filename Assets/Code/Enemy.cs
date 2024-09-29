using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        player = FindObjectOfType<Agent>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
