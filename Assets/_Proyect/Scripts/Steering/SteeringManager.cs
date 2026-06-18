using UnityEngine;
using System.Collections.Generic;

public abstract class SteeringBehavior : MonoBehaviour
{
    public bool isActive = true;
    public float weight = 1f;
    public abstract Vector3 CalculateForce();
}

public class SteeringManager : MonoBehaviour
{
    [Header("Settings")]
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;
    public float stopDistance = 0.5f;

    [Header("Debug")]
    public Vector3 velocity;
    public Vector3 steeringForce;
    public Vector3 target;
    public bool isMoving = false;

    private Rigidbody rb;
    private List<SteeringBehavior> behaviors = new List<SteeringBehavior>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.drag = 1f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        ApplySteering();
    }

    public void AddBehavior(SteeringBehavior behavior)
    {
        if (!behaviors.Contains(behavior))
        {
            behaviors.Add(behavior);
        }
    }

    // ? SOLO UNA VEZ (eliminé la duplicada)
    public void RemoveBehavior(SteeringBehavior behavior)
    {
        if (behaviors.Contains(behavior))
        {
            behaviors.Remove(behavior);
        }
    }

    public void ClearBehaviors()
    {
        behaviors.Clear();
    }

    void ApplySteering()
    {
        steeringForce = Vector3.zero;

        foreach (var behavior in behaviors)
        {
            if (behavior != null && behavior.isActive)
            {
                steeringForce += behavior.CalculateForce() * behavior.weight;
            }
        }

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        velocity = Vector3.ClampMagnitude(velocity + steeringForce / mass * Time.fixedDeltaTime, maxSpeed);

        if (target != null && Vector3.Distance(transform.position, target) < stopDistance)
        {
            velocity *= 0.9f;
            if (velocity.magnitude < 0.1f)
            {
                velocity = Vector3.zero;
                isMoving = false;
            }
        }
        else
        {
            isMoving = velocity.magnitude > 0.1f;
        }

        rb.velocity = velocity;

        if (velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void Stop()
    {
        velocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        isMoving = false;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
    }
}