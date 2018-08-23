using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    /// <summary>
    /// Click to move using CharacterController. 
    /// !!! Doesn't have pathfinding yet !!!
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ClickToMoveCharacter : ClickBase
    {
        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 8f;

        CharacterController controller;

        protected override void Start()
        {
            base.Start();
            controller = GetComponent<CharacterController>();
        }

        public override void Move()
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetPoint.x, transform.position.y, targetPoint.z) - transform.position);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 direction = targetPoint - transform.position;

            Debug.Log(direction + " / " + direction.magnitude + " / " + controller.isGrounded);

            //direction.y -= Physics.gravity.y * Time.deltaTime;
            if ((direction.magnitude - controller.height / 2f) > 0.2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                controller.SimpleMove(forward * moveSpeed);
            }
        }
    }
}
