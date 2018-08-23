using UnityEngine;
using System.Collections;

namespace Controls
{
    /// <summary>
    /// Base class for player movement
    /// TODO: make it abstract?
    /// </summary>
    public abstract class MoveBase : MonoBehaviour
    {

        void FixedUpdate()
        {
            GetInput();
            Move();
        }


        /// <summary>
        /// Gets player input from gamepad/keyboard/onscreen controls
        /// </summary>
        public abstract void GetInput();


        /// <summary>
        /// Move player character according to game mechanics
        /// </summary>
        public abstract void Move();
    }
}