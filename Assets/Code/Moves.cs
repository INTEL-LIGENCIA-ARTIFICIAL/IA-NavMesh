/*  Source: 
    Artificial Intelligence for Beginners
    Penny de Byl
    https://learn.unity.com/course/artificial-intelligence-for-beginners
*/

﻿using UnityEngine;
using UnityEngine.AI;

public class Moves : MonoBehaviour
{
    public GameObject target;
    public Collider floor;
    GameObject[] hidingSpots;
    NavMeshAgent agent;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    public void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));

        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

//        if ((toTarget > 90 && relativeHeading < 20) || ds.currentSpeed < 0.01f)
        if ((toTarget > 90 && relativeHeading < 20))
        {
            Seek(target.transform.position);
            return;
        }

//        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        float lookAhead = targetDir.magnitude / (agent.speed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    public void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
//        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        float lookAhead = targetDir.magnitude / agent.speed;
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    Vector3 wanderTarget = Vector3.zero;
    public void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        if (!floor.bounds.Contains(targetWorld))
        {
            targetWorld = -transform.position * 0.1f;

        };

        Seek(targetWorld);
    }

    public void Hide()
    {
        float closestDist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = null;

        // Encuentra el mejor lugar para esconderse
        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir = hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized * 100;

            float distToHidePos = Vector3.Distance(this.transform.position, hidePos);

            if (distToHidePos < closestDist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = hidingSpots[i];
                closestDist = distToHidePos;
            }
        }

        if (chosenGO == null)
        {
            Debug.LogWarning("No hiding spot found");
            return;
        }

        // Raycast para ajustar la posición final detrás del objeto de escondite
        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit hitInfo;
        float maxDistance = 250.0f;

        if (hideCol.Raycast(backRay, out hitInfo, maxDistance))
        {
            // Mueve el agente justo detrás del objeto, ajustando un poco la posición
            Vector3 hidePointBehind = hitInfo.point + chosenDir.normalized * 2.0f; // Ajuste de 2.0f para alejarse un poco
            Seek(hidePointBehind);
        }
        else
        {
            // Si el Raycast falla, ve al punto inicial de escondite calculado
            Seek(chosenSpot);
            Debug.Log("falla");
        }
    }

}
