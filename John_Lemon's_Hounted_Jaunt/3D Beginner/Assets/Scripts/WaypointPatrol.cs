using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // For enemies


public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;



    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position); //initial destination

    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) // < 0.2 -> true
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length; // 3 % 4 = 3 ; 5 % 4 = 1
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
