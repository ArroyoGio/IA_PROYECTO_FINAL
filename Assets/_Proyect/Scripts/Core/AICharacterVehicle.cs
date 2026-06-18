using UnityEngine;

public abstract class AICharacterVehicle : AICharacterControl
{
    [Header("Vehicle Settings")]
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;
    public Rigidbody rb;

    protected virtual void FixedUpdate()
    {
        UpdateAI();
    }

    public abstract void Move(Vector3 target);

    public virtual void Stop()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}