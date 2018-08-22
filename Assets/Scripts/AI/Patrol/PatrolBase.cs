using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.Patrol
{

    public class PatrolBase : MonoBehaviour
    {
        
        public float speed = 5f;
        public float turnSpeed = 90f;
        public float waitTime = 3f;
        public Transform[] waypoints;

        void OnDrawGizmos()
        {
            Vector3 startPosition = waypoints[0].position;
            Vector3 prevPosition = startPosition;
            foreach (Transform waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(prevPosition, waypoint.position);
                prevPosition = waypoint.position;
            }
            Gizmos.DrawLine(prevPosition, startPosition);
        }

        public virtual Vector3 WaypointPosition(int index)
        {
            return waypoints[index].position;
        }
    }
}