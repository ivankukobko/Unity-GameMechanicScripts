using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    /// <summary>
    /// Simple click to move with NavMeshAgent. 
    /// Don't forget to generate NavMesh
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMoveAgent : ClickBase
    {
        NavMeshAgent agent;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
        }

        public override void Move()
        {
            agent.SetDestination(targetPoint);
        }
    }
}