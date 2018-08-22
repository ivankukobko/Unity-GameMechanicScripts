using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{

    public class StickToMove : StickBase
    {

        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 8f;

        float angle;
        float smoothInputMagnitude;
        float smoothMoveVelocity;
        Vector3 inputDirection;

        void FixedUpdate()
        {
            GetInput();
            Move();
        }

        public override void GetInput()
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputDirection.magnitude, ref smoothMoveVelocity, smoothMoveTime);

            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputDirection.magnitude);

            velocity = transform.forward * moveSpeed * smoothInputMagnitude;
        }


        public override void Move()
        {
            rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }
}