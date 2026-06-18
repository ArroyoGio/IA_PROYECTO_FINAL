using UnityEngine;

public abstract class PreyVehicleLand : AICharacterVehicle
{
    [Header("Prey Vehicle Settings")]
    public float rotationSpeed = 5f;

    public override void Move(Vector3 target)
    {
        if (rb == null) return;

        Vector3 direction = (target - transform.position).normalized;
        Vector3 desiredVelocity = direction * maxSpeed;

        Vector3 steeringForce = desiredVelocity - rb.velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        rb.AddForce(steeringForce, ForceMode.Force);

        // Rotar hacia la dirección del movimiento
        if (rb.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public override void UpdateAI()
    {
        // Las clases hijas implementan su lógica específica
    }
}