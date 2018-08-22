using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    public class ClickToMove : MoveBase
    {
        public LayerMask moveableMask;
        public float raycastDistance = 1000f;

        new Camera camera;
        NavMeshAgent agent;
        Vector3 targetPoint;

        void Start()
        {
            camera = Camera.main;
            agent = GetComponent<NavMeshAgent>();
        }


        public override void GetInput () {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, raycastDistance, moveableMask))
                {
                    targetPoint = hit.point;
                }
            }
        }

        public override void Move()
        {
            agent.SetDestination(targetPoint);
        }
    }
}