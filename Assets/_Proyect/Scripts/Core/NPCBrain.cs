using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCBrain : MonoBehaviour
{
    [Header("=== MOVIMIENTO ===")]
    public float speed = 3f;
    public float wanderRadius = 10f;
    public float rotationSpeed = 5f;

    [Header("=== VISIÓN ===")]
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask targetLayer;

    [Header("=== SALUD ===")]
    public float maxHealth = 100f;
    public float health = 100f;
    public bool isImmortal = false;

    [Header("=== ESTADO ===")]
    public float hunger = 0f;
    public float energy = 100f;

    // Componentes
    private NavMeshAgent agent;
    private Blackboard blackboard;
    private Vector3 wanderTarget;
    private float timer = 0f;
    private Transform currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        blackboard = GetComponent<Blackboard>();
        if (agent != null) agent.speed = speed;
        wanderTarget = GetRandomPoint();
        health = maxHealth;
        UpdateBlackboard();
    }

    void Update()
    {
        timer += Time.deltaTime;
        hunger += Time.deltaTime * 0.5f;
        hunger = Mathf.Clamp(hunger, 0, 100);
        UpdateBlackboard();

        // Movimiento
        if (agent != null && agent.isActiveAndEnabled)
        {
            if (agent.remainingDistance < 0.5f && !agent.pathPending)
            {
                wanderTarget = GetRandomPoint();
                agent.SetDestination(wanderTarget);
            }
        }
    }

    // ========== MÉTODOS PÚBLICOS PARA EL BT ==========

    // --- MOVIMIENTO ---
    public void Wander()
    {
        if (agent != null)
        {
            wanderTarget = GetRandomPoint();
            agent.SetDestination(wanderTarget);
        }
    }

    public void Pursue(Transform target)
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
            currentTarget = target;
        }
    }

    public void Evade(Transform threat)
    {
        if (agent != null && threat != null)
        {
            Vector3 fleeDir = (transform.position - threat.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDir * 20f;
            agent.SetDestination(fleeTarget);
        }
    }

    public void Arrive(Vector3 target)
    {
        if (agent != null)
        {
            agent.SetDestination(target);
        }
    }

    public void Stop()
    {
        if (agent != null) agent.isStopped = true;
    }

    public void Resume()
    {
        if (agent != null) agent.isStopped = false;
    }

    public void MoveTo(Vector3 target)
    {
        if (agent != null) agent.SetDestination(target);
    }

    // --- VISIÓN ---
    public Transform GetNearestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
        Transform nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var t in targets)
        {
            Vector3 dir = (t.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle <= viewAngle / 2)
            {
                float dist = Vector3.Distance(transform.position, t.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = t.transform;
                }
            }
        }
        return nearest;
    }

    public bool HasTargets()
    {
        return GetNearestTarget() != null;
    }

    public bool IsTargetInRange(Transform target, float range)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < range;
    }

    // --- SALUD ---
    public void ApplyDamage(float damage)
    {
        if (isImmortal) return;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0) Die();
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void Die()
    {
        gameObject.SetActive(false);
        // Efectos de muerte
    }

    public bool IsDead() => health <= 0;
    public bool IsDangerHealth() => health < maxHealth * 0.5f;

    // --- HAMBRE Y ENERGÍA ---
    public void Eat(float amount)
    {
        hunger = Mathf.Max(0, hunger - amount);
        energy = Mathf.Min(100, energy + 10);
    }

    public void Rest()
    {
        energy += Time.deltaTime * 10f;
        energy = Mathf.Clamp(energy, 0, 100);
    }

    public bool IsHungry() => hunger > 70f;
    public bool IsTired() => energy < 30f;

    // --- UTILIDADES ---
    Vector3 GetRandomPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas);
        return hit.position;
    }

    void UpdateBlackboard()
    {
        if (blackboard == null) return;

        blackboard.SetFloat("Health", health);
        blackboard.SetFloat("Hunger", hunger);
        blackboard.SetFloat("Energy", energy);
        blackboard.SetBool("IsDead", health <= 0);
        blackboard.SetBool("IsDanger", health < maxHealth * 0.5f);
        blackboard.SetBool("IsHungry", hunger > 70f);
        blackboard.SetBool("IsTired", energy < 30f);
        blackboard.SetBool("HasTargets", HasTargets());

        Transform target = GetNearestTarget();
        if (target != null)
        {
            blackboard.SetObject("Target", target);
            blackboard.SetBool("TargetInRange", IsTargetInRange(target, 5f));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}