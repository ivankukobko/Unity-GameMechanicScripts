﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class FieldOfView : MonoBehaviour
    {

        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public LayerMask targetMask;
        public LayerMask obstacleMask;

        [HideInInspector]
        public List<Transform> visibleTargets;

        public float meshResolution;
        public int edgeResolveIterations;
        public float edgeDistanceThreshold;

        public MeshFilter viewMeshFilter;
        Mesh viewMesh;

        void Start()
        {
            visibleTargets = new List<Transform>();
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
            StartCoroutine("FindTargetsWithDelay", .2f);
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        void LateUpdate()
        {
            DrawFieldOfView();
        }

        public void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            ViewCastInfo oldViewCast = new ViewCastInfo();
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                //Debug.DrawLine(transform.position, transform.position + DirectionFromAngle (angle, true) * viewRadius, Color.white);
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                    if (oldViewCast.hit != newViewCast.hit ||
                        oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.pointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointA);
                        }
                        if (edge.pointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointB);
                        }
                    }
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 1; i < vertexCount; i++)
            {
                vertices[i] = transform.InverseTransformPoint(viewPoints[i - 1]);
                if (i < vertexCount - 2)
                {
                    triangles[(i - 1) * 3 + 0] = 0;
                    triangles[(i - 1) * 3 + 1] = i;
                    triangles[(i - 1) * 3 + 2] = i + 1;
                }
            }

            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;

            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;

                }
            }
            return new EdgeInfo(minPoint, maxPoint);
        }

        ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 direction = DirectionFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
            }
        }


        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (!isGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }


        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float distance;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
            {
                hit = _hit;
                point = _point;
                distance = _distance;
                angle = _angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }
    }
}