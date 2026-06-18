using UnityEngine;

    public class OffsetPursuit : SteeringBehavior
    {
        [Header("Offset Pursuit Settings")]
        public Transform leader;
        public Vector3 offset;
        public float pursuitStrength = 1f;

        private SteeringManager manager;

        void Start()
        {
            manager = GetComponent<SteeringManager>();
        }

        public override Vector3 CalculateForce()
        {
            if (leader == null || manager == null) return Vector3.zero;

            Vector3 targetOffset = leader.position + leader.TransformDirection(offset);
            Vector3 desired = (targetOffset - transform.position).normalized * manager.maxSpeed;
            Vector3 steering = desired - manager.velocity;

            return steering * pursuitStrength;
        }

        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
        }

        public void SetLeader(Transform newLeader)
        {
            leader = newLeader;
        }
    }
