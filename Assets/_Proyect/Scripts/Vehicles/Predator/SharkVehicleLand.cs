using UnityEngine;

public class SharkVehicleLand : PredatorVehicleLand
{
    [Header("Shark Vehicle")]
    public float huntingSpeedMultiplier = 2f;
    public float biteRange = 3f;

    private Pursuit pursuit;

    protected override void Awake()
    {
        base.Awake();
        pursuit = GetComponent<Pursuit>();
    }

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void Hunt(Transform prey)
    {
        if (prey != null)
        {
            // Usar Pursuit para interceptar a la presa
            if (pursuit != null)
            {
                pursuit.SetTarget(prey);
                pursuit.isActive = true;
            }
            Move(prey.position);
        }
    }

    public void Patrol(Vector3 patrolPoint)
    {
        Move(patrolPoint);
    }

    public bool IsInBiteRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < biteRange;
    }
}