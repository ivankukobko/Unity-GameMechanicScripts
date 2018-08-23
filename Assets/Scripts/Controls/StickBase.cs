using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    /// <summary>
    /// Base class for simple movement using joystick/keys
    /// </summary>
    public class StickBase : MoveBase
    {
        [HideInInspector]
        public float inputH, inputV;
        public Vector3 velocity;

        public override void GetInput()
        {
            inputH = Input.GetAxisRaw("Horizontal");
            inputV = Input.GetAxisRaw("Vertical");
        }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}