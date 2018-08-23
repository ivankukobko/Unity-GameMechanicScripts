using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    /// <summary>
    /// Simple implementation of player movement with joystick/keys control.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class StickToMoveCharacter: StickBase
    {

        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 8f;

        float angle;
        float smoothInputMagnitude;
        float smoothMoveVelocity;
        Vector3 inputDirection;
        CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        public override void Move()
        {
            inputDirection = new Vector3(inputH, 0f, inputV).normalized;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputDirection.magnitude, ref smoothMoveVelocity, smoothMoveTime);

            velocity = inputDirection * moveSpeed * smoothInputMagnitude;
            velocity.y += Physics.gravity.y * Time.deltaTime;

            if (inputDirection.magnitude > 0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDirection), turnSpeed * Time.deltaTime);
                controller.Move(velocity * Time.deltaTime);
            }
        }
    }
}