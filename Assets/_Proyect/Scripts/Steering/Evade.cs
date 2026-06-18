using UnityEngine;

public class Evade : SteeringBehavior
{
    [Header("Evade Settings")]
    public Transform threat;
    public float predictionTime = 2f;
    public float evadeStrength = 1.5f;

    private SteeringManager manager;

    void Start()
    {
        manager = GetComponent<SteeringManager>();
    }

    public override Vector3 CalculateForce()
    {
        if (threat == null || manager == null) return Vector3.zero;

        Rigidbody threatRb = threat.GetComponent<Rigidbody>();
        Vector3 threatVelocity = threatRb != null ? threatRb.velocity : Vector3.zero;

        Vector3 futurePosition = threat.position + threatVelocity * predictionTime;

        Vector3 fleeDirection = (transform.position - futurePosition).normalized;
        Vector3 desired = fleeDirection * manager.maxSpeed;

        Vector3 steering = desired - manager.velocity;
        return steering * evadeStrength;
    }

    public void SetThreat(Transform newThreat)
    {
        threat = newThreat;
    }
}