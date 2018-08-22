using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{

    [RequireComponent(typeof(Rigidbody))]
    public class StickBase : MoveBase
    {
        public Rigidbody rb;
        public Vector3 velocity;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}