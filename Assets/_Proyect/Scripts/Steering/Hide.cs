using UnityEngine;

public class Hide : SteeringBehavior
{
    [Header("Hide Settings")]
    public Transform threat;
    public LayerMask obstacleLayer;
    public float hideRadius = 10f;
    public float hideStrength = 1f;

    private SteeringManager manager;
    private Transform bestHidingSpot;

    void Start()
    {
        manager = GetComponent<SteeringManager>();
    }

    public override Vector3 CalculateForce()
    {
        if (threat == null || manager == null) return Vector3.zero;

        FindBestHidingSpot();

        if (bestHidingSpot == null)
        {
            // Si no hay escondite, usar Evade (Steering Behavior)
            Evade evade = GetComponent<Evade>();  //AHORA USA "Evade", NO "EvadeNode"
            if (evade != null)
            {
                evade.SetThreat(threat);
                return evade.CalculateForce();
            }
            return Vector3.zero;
        }

        Vector3 desired = (bestHidingSpot.position - transform.position).normalized * manager.maxSpeed;
        Vector3 steering = desired - manager.velocity;

        return steering * hideStrength;
    }

    void FindBestHidingSpot()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, hideRadius, obstacleLayer);
        float bestDistance = float.MaxValue;
        bestHidingSpot = null;

        foreach (var obstacle in obstacles)
        {
            Vector3 hidePosition = obstacle.transform.position + (obstacle.transform.position - threat.position).normalized * 2f;

            if (!IsPositionValid(hidePosition)) continue;

            float distance = Vector3.Distance(transform.position, hidePosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestHidingSpot = obstacle.transform;
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        Vector3 directionToThreat = threat.position - position;

        if (Physics.Raycast(position, directionToThreat, directionToThreat.magnitude, obstacleLayer))
        {
            return true;
        }

        return false;
    }

    public void SetThreat(Transform newThreat)
    {
        threat = newThreat;
    }
}