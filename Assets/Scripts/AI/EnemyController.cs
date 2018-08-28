using System.Collections;

using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    Idle, Patrol, Chase, Attack
}

namespace AI
{

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    public class EnemyController : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public CharacterController character { get; private set; } // the character we are controlling
        public Vector3? targetPosition = null; // target to aim for
        public Transform checkpoint;
        private EnemySight sight;
        [SerializeField]
        private EnemyState currentState;

        EnemyState CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;

                StopAllCoroutines();

                switch (currentState)
                {
                    case EnemyState.Idle:
                        StartCoroutine(AI_Idle());
                        break;

                    case EnemyState.Patrol:
                        StartCoroutine(AI_Patrol());
                        break;

                    case EnemyState.Chase:
                        StartCoroutine(AI_Chase());
                        break;

                    case EnemyState.Attack:
                        StartCoroutine(AI_Attack());
                        break;
                }

            }
        }

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<CharacterController>();
            sight = GetComponent<EnemySight>();
            agent.updateRotation = false;
            agent.updatePosition = true;

            currentState = EnemyState.Idle;
            StartCoroutine(AI_Idle());
        }

        IEnumerator AI_Idle()
        {
            while (CurrentState == EnemyState.Idle)
            {

                // Try to chase if there's target
                if (targetPosition != null)
                {
                    CurrentState = EnemyState.Chase;
                    yield break;
                }

                // Try to patrol if checkpoint available
                if (checkpoint)
                {
                    CurrentState = EnemyState.Patrol;
                    yield break;
                }

                yield return null;
            }
        }

        IEnumerator AI_Patrol()
        {
            while (CurrentState == EnemyState.Patrol)
            {
                if (checkpoint != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(checkpoint.position);
                }
                else
                {
                    agent.isStopped = true;
                    CurrentState = EnemyState.Idle;
                }

                while (agent.pathPending)
                    yield return null;

                MoveToTarget();

                if (sight.currentTarget != null)
                {
                    agent.isStopped = true;
                    CurrentState = EnemyState.Chase;
                    yield break;
                }

                yield return null;
            }
        }

        IEnumerator AI_Chase()
        {
            while (CurrentState == EnemyState.Chase)
            {
                if (sight.lastSeenPosition != null)
                    targetPosition = sight.lastSeenPosition;

                if (sight.currentTarget)
                    targetPosition = sight.currentTarget.position;

                if (targetPosition != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination((Vector3)targetPosition);
                }

                while (agent.pathPending)
                    yield return null;

                MoveToTarget();

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.isStopped = true;
                    CurrentState = sight.CanSeeTarget ? EnemyState.Attack : EnemyState.Patrol;
                    yield break;
                }

                yield return null;
            }
        }

        IEnumerator AI_Attack()
        {
            while (CurrentState == EnemyState.Attack)
            {
                if (sight.currentTarget)
                {
                    agent.isStopped = false;
                    agent.SetDestination(sight.currentTarget.position);
                }

                while (agent.pathPending)
                    yield return null;

                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    CurrentState = EnemyState.Chase;
                    yield break;
                }
                else
                {
                    Debug.Log("Attacking target!");
                }

                yield return null;
            }
        }

        public void SetTargetPosition(Vector3 newPosition)
        {
            this.targetPosition = newPosition;
        }

        private void MoveToTarget()
        {
            Vector3 moveTarget = (agent.remainingDistance > agent.stoppingDistance) ? agent.desiredVelocity : Vector3.zero;
            agent.SetDestination(moveTarget);
        }
    }
}