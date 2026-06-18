using UnityEngine;
public enum NPCType
{
    Fish, Dolphin, Shark, Snake, Kraken, Octopus, Lobster, Diver, Sphere
}

public class FuzzyController : MonoBehaviour
{
    [Header("Fuzzy Settings")]
    public AnimationCurve distanceToSpeed;
    public AnimationCurve healthToAggressiveness;
    public AnimationCurve hungerToSpeed;

    [Header("Input Values")]
    public float currentDistance = 0f;
    public float currentHealth = 100f;
    public float currentHunger = 0f;

    [Header("Output Values")]
    public float speedMultiplier = 1f;
    public float aggressivenessMultiplier = 1f;

    private SteeringManager steering;
    private HealthBase health;
    private Blackboard blackboard;

    public NPCType npcType;


    void Start()
    {
        steering = GetComponent<SteeringManager>();
        health = GetComponent<HealthBase>();
        blackboard = GetComponent<Blackboard>();
    }

    void Update()
    {
        // Obtener valores del Blackboard
        if (blackboard != null)
        {
            currentHunger = blackboard.GetFloat("Hunger", 0f);
            currentHealth = health != null ? health.HealthPercentage * 100f : 100f;

            // Detectar distancia a la presa más cercana
            AIEyeBase eye = GetComponent<AIEyeBase>();
            if (eye != null)
            {
                Transform target = eye.GetNearestTarget();
                if (target != null)
                {
                    currentDistance = Vector3.Distance(transform.position, target.position);
                }
                else
                {
                    currentDistance = 20f;
                }
            }
        }

        // Calcular multiplicadores usando Fuzzy Logic
        speedMultiplier = distanceToSpeed.Evaluate(currentDistance);
        aggressivenessMultiplier = healthToAggressiveness.Evaluate(currentHealth);

        // Aplicar al SteeringManager
        if (steering != null)
        {
            // Velocidad base * multiplicador difuso
            float baseSpeed = GetBaseSpeed();
            steering.maxSpeed = baseSpeed * speedMultiplier;

            // Si está cazando, aumentar velocidad
            if (blackboard != null && blackboard.GetBool("IsHunting", false))
            {
                steering.maxSpeed *= 1.5f;
            }
        }
    }

    float GetBaseSpeed()
    {
        switch (npcType)
        {
            case NPCType.Fish: return 3f;
            case NPCType.Dolphin: return 5f;
            case NPCType.Shark: return 4f;
            case NPCType.Snake: return 3f;
            case NPCType.Kraken: return 2f;
            case NPCType.Octopus: return 2f;
            case NPCType.Lobster: return 1.5f;
            case NPCType.Diver: return 3f;
            default: return 3f;
        }
    }

    public float GetSpeedMultiplier() => speedMultiplier;
    public float GetAggressivenessMultiplier() => aggressivenessMultiplier;
}