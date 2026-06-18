using UnityEngine;

public abstract class AmbientVehicleLand : AICharacterVehicle
{
    [Header("Ambient Vehicle Settings")]
    public bool isStationary = true;

    public override void Move(Vector3 target)
    {
        // Los objetos ambientales no se mueven
        if (!isStationary)
        {
            // Si se mueven, usar movimiento base
            if (rb == null) return;
            Vector3 direction = (target - transform.position).normalized;
            Vector3 desiredVelocity = direction * maxSpeed;
            Vector3 steeringForce = desiredVelocity - rb.velocity;
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
            rb.AddForce(steeringForce, ForceMode.Force);
        }
    }

    public override void UpdateAI()
    {
        // Los objetos ambientales no necesitan actualizar IA
    }
}