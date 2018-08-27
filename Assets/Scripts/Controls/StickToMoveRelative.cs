using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    /// <summary>
    /// Simple implementation of player movement with joystick/keys control.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class StickToMoveRelative : StickBase
    {

        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 180f;

        CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        public override void Move()
        {
            transform.rotation *= Quaternion.Euler(new Vector3(0f, turnSpeed * Time.deltaTime * inputH, 0f));
            velocity.z = inputV * moveSpeed;

            velocity.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(transform.TransformDirection(velocity) * Time.deltaTime);
        }
    }
}