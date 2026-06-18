using UnityEngine;

    public class Wander : SteeringBehavior
    {
        [Header("Wander Settings")]
        public float wanderRadius = 3f;
        public float wanderDistance = 5f;
        public float wanderJitter = 0.5f;
        public float wanderSpeed = 1f;

        private Vector3 wanderTarget;
        private SteeringManager manager;
        private float timer = 0f;

        void Start()
        {
            manager = GetComponent<SteeringManager>();
            wanderTarget = Random.insideUnitSphere * wanderRadius;
            wanderTarget.y = 0;
        }

        public override Vector3 CalculateForce()
        {
            if (manager == null) return Vector3.zero;

            timer += Time.deltaTime * wanderSpeed;

            wanderTarget += new Vector3(
                Mathf.Sin(timer * 1.3f) * wanderJitter,
                0,
                Mathf.Cos(timer * 0.7f) * wanderJitter
            );
            wanderTarget = wanderTarget.normalized * wanderRadius;

            Vector3 forward = transform.forward;
            Vector3 targetLocal = forward * wanderDistance + wanderTarget;
            Vector3 worldTarget = transform.position + targetLocal;

            Vector3 desired = (worldTarget - transform.position).normalized * manager.maxSpeed;
            Vector3 steering = desired - manager.velocity;

            return steering * 0.5f;
        }

        void OnDrawGizmosSelected()
        {
            if (!isActive) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + transform.forward * wanderDistance, wanderRadius);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * wanderDistance + wanderTarget);
        }
    }