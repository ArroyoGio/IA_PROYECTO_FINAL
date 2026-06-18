using UnityEngine;

    public class Pursuit : SteeringBehavior
    {
        [Header("Pursuit Settings")]
        public Transform target;
        public float predictionTime = 2f;
        public float pursuitStrength = 1.5f;

        private SteeringManager manager;

        void Start()
        {
            manager = GetComponent<SteeringManager>();
        }

        public override Vector3 CalculateForce()
        {
            if (target == null || manager == null) return Vector3.zero;

            // Predecir posición futura del objetivo
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            Vector3 targetVelocity = targetRb != null ? targetRb.velocity : Vector3.zero;

            Vector3 futurePosition = target.position + targetVelocity * predictionTime;

            // Ir hacia la posición futura
            Vector3 desired = (futurePosition - transform.position).normalized * manager.maxSpeed;
            Vector3 steering = desired - manager.velocity;

            return steering * pursuitStrength;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
