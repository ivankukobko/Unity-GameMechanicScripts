using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Patrol
{
    public class PatrolDefault : PatrolBase
    {
        public bool ignoreWaypointY = true;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            transform.position = WaypointPosition(0);

            int targetWaypointIndex = 1;
            Vector3 targetWaypoint = WaypointPosition(targetWaypointIndex);
            transform.LookAt(targetWaypoint);

            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

                if (transform.position == targetWaypoint)
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = WaypointPosition(targetWaypointIndex);
                    yield return new WaitForSeconds(waitTime);

                    yield return StartCoroutine(TurnToFace(targetWaypoint));
                }
                yield return null;
            }
        }

        IEnumerator TurnToFace(Vector3 lookTarget)
        {
            Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
            float targetAngle = 90 - Mathf.Atan2(directionToLookTarget.z, directionToLookTarget.x) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.01f)
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
        }

        public override Vector3 WaypointPosition(int index) {
            Vector3 pos = waypoints[index].position;
            return new Vector3(pos.x, (ignoreWaypointY ? transform.position.y : pos.y), pos.z);
        }
    }
}