using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    /// <summary>
    /// Simple player movement/jump on a grid, like in puzzles and turn-based games
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class StickToStep : StickBase
    {
        public float stepSize = 2f;
        public float moveSpeed = .5f;
        public bool snapToGrid = true;

        float moveTimeout = 0f;
        Vector3 targetPosition;
        CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        public override void Move()
        {
            CheckDirection(); 
            StartCoroutine(MoveRoutine());
        }

        IEnumerator MoveRoutine () {
            Debug.Log(velocity + " / " + moveTimeout + "/ " + inputH + ":" + inputV);

            if (Mathf.Abs(velocity.x) > 0f || Mathf.Abs(velocity.z) > 0f) {
                if (moveTimeout <= 0f)
                {
                    moveTimeout = 1 / moveSpeed;

                    Vector3 snapPos;
                    snapPos.x = Mathf.Round(transform.position.x / stepSize) * stepSize;
                    snapPos.y = Mathf.Round(transform.position.y / stepSize) * stepSize;
                    snapPos.z = Mathf.Round(transform.position.z / stepSize) * stepSize;

                    transform.position = new Vector3(snapPos.x, transform.position.y, snapPos.z);
                }

                while (moveTimeout > 0f) {
                    moveTimeout -= Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
                    controller.Move(velocity * Time.deltaTime * moveSpeed);
                    yield return null;
                }
            }
            yield return null;
        }

        void CheckDirection () {
            if (moveTimeout > 0f) return;
    
            float x = ToStepSize(FirstIfGreater(inputH, inputV));
            float z = ToStepSize(FirstIfGreater(inputV, inputH));
            velocity = new Vector3(x, 0f, z);
            velocity.y += Physics.gravity.y;
        }

        float FirstIfGreater(float first, float second) {
            return Mathf.Abs(first) > Mathf.Abs(second) ? first : 0f;
        }

        float ToStepSize (float value) {
            bool positive = value > 0f;
            return Mathf.Abs(value) > 0f ? (positive? 1f : -1f) * stepSize : 0f;
        }

    }
}