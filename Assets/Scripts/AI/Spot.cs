using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Spot : MonoBehaviour
    {
        public static event System.Action OnTargetSpotted;

        public Light spotlight;
        public float viewDistance;
        public LayerMask viewMask;
        public float timeToSpotPlayer = .5f;

        Transform target;
        Color originalSpotlightColor;
        float viewAngle;
        float targetSpottedTimer;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            viewAngle = spotlight.spotAngle;
            originalSpotlightColor = spotlight.color;
        }

        void Update()
        {
            targetSpottedTimer += Time.deltaTime * (CanSeeTarget() ? 1 : -1);
            targetSpottedTimer = Mathf.Clamp(targetSpottedTimer, 0f, timeToSpotPlayer);
            spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, targetSpottedTimer / timeToSpotPlayer);
            if (targetSpottedTimer >= timeToSpotPlayer)
            {
                if (OnTargetSpotted != null)
                {
                    OnTargetSpotted();
                }
            }

        }
        bool CanSeeTarget()
        {
            if (Vector3.Distance(transform.position, target.position) < viewDistance)
            {
                Vector3 dirToPlayer = (target.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
                if (angleBetweenGuardAndPlayer < viewAngle / 2f)
                {
                    if (!Physics.Linecast(transform.position, target.position, viewMask))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        }
    }
}