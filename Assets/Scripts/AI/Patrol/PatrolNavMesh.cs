using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Patrol
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrolNavMesh : PatrolBase
    {
        NavMeshAgent agent;
        // Use this for initialization
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            int targetWaypointIndex = 0;
            Vector3 targetWaypoint = WaypointPosition(targetWaypointIndex);

            while (true)
            {
                agent.SetDestination(targetWaypoint);

                if (transform.position == targetWaypoint)
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = WaypointPosition(targetWaypointIndex);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return null;
            }
        }
    }
}