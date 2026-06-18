using UnityEngine;

public class LobsterVehicleLand : SurvivorVehicleLand
{
    [Header("Lobster Vehicle")]
    public float burrowSpeed = 2f;

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void Burrow(Vector3 burrowPosition)
    {
        float originalSpeed = maxSpeed;
        maxSpeed = burrowSpeed;
        Move(burrowPosition);
        maxSpeed = originalSpeed;
    }

    public void AmbushMove(Transform prey)
    {
        if (prey != null)
        {
            // Moverse lentamente hacia la presa
            float originalSpeed = maxSpeed;
            maxSpeed = 1f;
            Move(prey.position);
            maxSpeed = originalSpeed;
        }
    }

    public void ScuttleAway(Vector3 escapeDirection)
    {
        Vector3 escapeTarget = transform.position + escapeDirection * 10f;
        Move(escapeTarget);
    }
}