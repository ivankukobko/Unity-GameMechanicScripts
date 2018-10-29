using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace AI.Patrol
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrolAroundPoint : PatrolBase
    {
        public float radius = 10f;
        public Vector3 pointOfInterest;
        NavMeshAgent agent;
        Vector3 targetWaypoint;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;

            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            targetWaypoint = RandomPointOnCircleEdge();

            while (true)
            {
                agent.SetDestination(targetWaypoint);

                if (Vector3.Distance(transform.position, targetWaypoint) < Mathf.Max(agent.stoppingDistance, 2f))
                {
                    yield return new WaitForSeconds(waitTime);
                    targetWaypoint = GenerateNextPoint();
                }

                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(pointOfInterest, .1f);
            Gizmos.DrawSphere(targetWaypoint, .5f);

            Gizmos.DrawWireSphere(pointOfInterest, radius);
        }

        private Vector3 RandomPointOnCircleEdge()
        {
            var vector2 = Random.insideUnitCircle.normalized * radius;
            return (new Vector3(vector2.x, 0f, vector2.y)) + pointOfInterest;
        }

        private Vector3 GenerateNextPoint()
        {
            Vector3 newPoint;

            do
            {
                newPoint = RandomPointOnCircleEdge();
            } while (Vector3.Distance(newPoint, targetWaypoint) < radius);

            return newPoint;
        }
    }
}