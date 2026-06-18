using UnityEngine;

    public class Arrive : SteeringBehavior
    {
        [Header("Arrive Settings")]
        public Transform target;
        public float slowingDistance = 5f;
        public float arriveStrength = 1f;

        private SteeringManager manager;

        void Start()
        {
            manager = GetComponent<SteeringManager>();
        }

        public override Vector3 CalculateForce()
        {
            if (target == null || manager == null) return Vector3.zero;

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < 0.1f)
            {
                manager.Stop();
                return Vector3.zero;
            }

            float speed = manager.maxSpeed;

            // Reducir velocidad al acercarse
            if (distance < slowingDistance)
            {
                speed = manager.maxSpeed * (distance / slowingDistance);
                speed = Mathf.Max(speed, 0.5f);
            }

            Vector3 desired = (target.position - transform.position).normalized * speed;
            Vector3 steering = desired - manager.velocity;

            return steering * arriveStrength;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }