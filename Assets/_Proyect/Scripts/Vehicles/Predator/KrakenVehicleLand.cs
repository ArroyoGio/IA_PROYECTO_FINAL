using UnityEngine;

public class KrakenVehicleLand : PredatorVehicleLand
{
    [Header("Kraken Vehicle")]
    public float wakeSpeed = 3f;
    public float chaseSpeed = 2f;
    public float tentacleRange = 15f;

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void MoveToPrey(Transform prey)
    {
        if (prey != null)
        {
            Move(prey.position);
        }
    }

    public void Retreat()
    {
        Vector3 retreatPosition = transform.position + transform.forward * -10f;
        Move(retreatPosition);
    }

    public bool IsInTentacleRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < tentacleRange;
    }

    public void WakeUpMove(Vector3 target)
    {
        float originalSpeed = maxSpeed;
        maxSpeed = wakeSpeed;
        Move(target);
        maxSpeed = originalSpeed;
    }
}