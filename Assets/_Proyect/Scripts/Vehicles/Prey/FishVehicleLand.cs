using UnityEngine;

public class FishVehicleLand : PreyVehicleLand
{
    [Header("Fish Vehicle")]
    public float schoolFormationStrength = 0.5f;

    private Wander wander;
    private Evade evade;
    private OffsetPursuit offsetPursuit;

    protected override void Awake()
    {
        base.Awake();
        wander = GetComponent<Wander>();
        evade = GetComponent<Evade>();
        offsetPursuit = GetComponent<OffsetPursuit>();
    }

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
        // Este método se llama desde FixedUpdate
    }

    public void MoveToSchool(Vector3 schoolCenter)
    {
        Move(schoolCenter);
    }

    public void FleeFromPredator(Transform predator)
    {
        if (predator != null)
        {
            Vector3 fleeDirection = (transform.position - predator.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * 20f;
            Move(fleeTarget);
        }
    }

    public void SwimRandomly()
    {
        if (wander != null && wander.isActive)
        {
            // Wander ya maneja el movimiento aleatorio
        }
    }
}