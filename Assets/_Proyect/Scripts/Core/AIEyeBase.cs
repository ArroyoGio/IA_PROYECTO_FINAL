using UnityEngine;
using System.Collections.Generic;

public abstract class AIEyeBase : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    [Header("Debug")]
    public bool showGizmos = true;
    protected List<Transform> visibleTargets = new List<Transform>();
    protected Blackboard blackboard;

    protected virtual void Awake()
    {
        blackboard = GetComponent<Blackboard>();
    }

    protected virtual void Update()
    {
        FindVisibleTargets();
        UpdateBlackboard();
    }

    protected virtual void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);

        foreach (var target in targetsInRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, targetTransform.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distance, obstacleLayer))
                {
                    visibleTargets.Add(targetTransform);
                }
            }
        }
    }

    protected abstract void UpdateBlackboard();

    public Transform GetNearestTarget()
    {
        Transform nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (var target in visibleTargets)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = target;
                }
            }

            return nearest;
        }

        public bool CanSee(Transform target)
        {
            return visibleTargets.Contains(target);
        }

        public bool HasTargets()
        {
            return visibleTargets.Count > 0;
        }

        void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 leftAngle = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewRadius;
            Vector3 rightAngle = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewRadius;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, leftAngle);
            Gizmos.DrawRay(transform.position, rightAngle);

            Gizmos.color = Color.green;
            foreach (var target in visibleTargets)
            {
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }