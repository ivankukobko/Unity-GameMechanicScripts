using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    /// <summary>
    /// Base class for click/tap to move player controls.
    /// </summary>
    public class ClickBase : MoveBase
    {
        public LayerMask moveableMask;
        public float raycastDistance = 1000f;

        protected Vector3 targetPoint;
        protected new Camera camera;

        protected virtual void Start()
        {
            camera = Camera.main;
        }


        public override void GetInput()
        {
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
            throw new System.NotImplementedException();
        }
    }
}