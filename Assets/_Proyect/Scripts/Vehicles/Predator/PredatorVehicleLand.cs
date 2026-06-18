using UnityEngine;

public abstract class PredatorVehicleLand : AICharacterVehicle
{
    [Header("Predator Vehicle Settings")]
    public float rotationSpeed = 5f;
    public float attackSpeedMultiplier = 1.5f;

    public override void Move(Vector3 target)
    {
        if (rb == null) return;

        Vector3 direction = (target - transform.position).normalized;
        float currentMaxSpeed = maxSpeed;

        // Si está atacando, aumentar velocidad
        if (blackboard != null && blackboard.GetBool("IsAttacking", false))
        {
            currentMaxSpeed *= attackSpeedMultiplier;
        }

        Vector3 desiredVelocity = direction * currentMaxSpeed;
        Vector3 steeringForce = desiredVelocity - rb.velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        rb.AddForce(steeringForce, ForceMode.Force);

        if (rb.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void AttackMove(Transform target)
    {
        if (target != null)
        {
            Move(target.position);
        }
    }

    public override void UpdateAI()
    {
        // Las clases hijas implementan su lógica específica
    }
}