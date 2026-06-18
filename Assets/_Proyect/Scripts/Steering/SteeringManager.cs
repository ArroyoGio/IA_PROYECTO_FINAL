using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

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
    private NavMeshAgent agent;
    private List<SteeringBehavior> behaviors = new List<SteeringBehavior>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.drag = 1f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Aplicar steering y mover al agente
        ApplySteeringToAgent();
    }

    public void AddBehavior(SteeringBehavior behavior)
    {
        if (!behaviors.Contains(behavior))
        {
            behaviors.Add(behavior);
        }
    }

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
        velocity = Vector3.ClampMagnitude(velocity + steeringForce / mass * Time.deltaTime, maxSpeed);

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

    public void ApplySteeringToAgent()
    {
        if (agent == null) return;

        // Calcular la fuerza de steering
        ApplySteering();

        // Aplicar la velocidad calculada al NavMeshAgent
        if (velocity.magnitude > 0.1f)
        {
            Vector3 targetPosition = transform.position + velocity * Time.deltaTime;
            agent.SetDestination(targetPosition);
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
        if (agent != null) agent.isStopped = true;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
    }
}