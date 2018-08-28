using UnityEngine;
using System.Collections;

namespace AI
{
    [RequireComponent(typeof(FieldOfView))]
    public class EnemySight : MonoBehaviour
    {
        private FieldOfView fov = null;
        public Transform currentTarget = null;
        public Vector3? lastSeenPosition = null;
        private bool canSeeTarget = false;

        public bool CanSeeTarget { get { return canSeeTarget; } }

        // Use this for initialization
        void Start()
        {
            fov = GetComponent<FieldOfView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTarget)
            {
                lastSeenPosition = currentTarget.position;
                canSeeTarget = true;
            }
            else
            {
                canSeeTarget = false;
            }
            currentTarget = fov.visibleTargets.Count > 0 ? fov.visibleTargets[0] : null;
        }
    }

}