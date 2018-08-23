using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    /// <summary>
    /// Simple implementation of player movement with joystick/keys control.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class StickToMoveRigidBody : StickBase
    {

        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 8f;

        float angle;
        float smoothInputMagnitude;
        float smoothMoveVelocity;
        Vector3 inputDirection;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void Move()
        {
            inputDirection = new Vector3(inputH, 0f, inputV).normalized;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputDirection.magnitude, ref smoothMoveVelocity, smoothMoveTime);

            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputDirection.magnitude);

            velocity = transform.forward * moveSpeed * smoothInputMagnitude;

            rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }
}