using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    /// <summary>
    /// Click to move with Rigidbody.
    /// !!! Doesn't have pathfinding logic !!!
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ClickToMoveRigidbody : ClickBase
    {
        public float moveSpeed = 7f;
        public float smoothMoveTime = .1f;
        public float turnSpeed = 8f;
        new Rigidbody rigidbody;

        float angle;
        float smoothInputMagnitude;
        float smoothMoveVelocity;
        Vector3 inputDirection;

        protected override void Start()
        {
            base.Start();
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Move()
        {
            float targetAngle = Mathf.Atan2(targetPoint.x, targetPoint.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed);

            //Vector3 velocity = Vector3.Lerp(transform.position, new Vector3(targetPoint.x, transform.position.y, targetPoint.z), Time.deltaTime * moveSpeed);

            rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            rigidbody.MovePosition(Vector3.MoveTowards(rigidbody.position, new Vector3(targetPoint.x, rigidbody.position.y, targetPoint.z), Time.deltaTime * moveSpeed));
        }
    }
}
