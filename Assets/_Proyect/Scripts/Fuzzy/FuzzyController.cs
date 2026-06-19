using UnityEngine;

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

    //private SteeringManager steering;
    private HealthBase health;
    private Blackboard blackboard;

    void Start()
    {
        //steering = GetComponent<SteeringManager>();
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
        //if (steering != null)
        //{
        //    // Velocidad base * multiplicador difuso
        //    float baseSpeed = GetBaseSpeed();
        //    steering.maxSpeed = baseSpeed * speedMultiplier;

        //    // Si está cazando, aumentar velocidad
        //    if (blackboard != null && blackboard.GetBool("IsHunting", false))
        //    {
        //        steering.maxSpeed *= 1.5f;
        //    }
        //}
    }

    //float GetBaseSpeed()
    //{
    //    // Obtener velocidad base según el tipo de NPC
    //    if (GetComponent<FishActionLand>() != null) return 3f;
    //    if (GetComponent<DolphinActionLand>() != null) return 5f;
    //    if (GetComponent<SharkActionLand>() != null) return 4f;
    //    if (GetComponent<SnakeActionLand>() != null) return 3f;
    //    if (GetComponent<KrakenActionLand>() != null) return 2f;
    //    if (GetComponent<OctopusActionLand>() != null) return 2f;
    //    if (GetComponent<LobsterActionLand>() != null) return 1.5f;
    //    if (GetComponent<DiverActionLand>() != null) return 3f;
    //    return 3f;
    //}

    public float GetSpeedMultiplier() => speedMultiplier;
    public float GetAggressivenessMultiplier() => aggressivenessMultiplier;
}