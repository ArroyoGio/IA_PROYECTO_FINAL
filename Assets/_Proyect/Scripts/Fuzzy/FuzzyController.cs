using UnityEngine;

public class FuzzyController : MonoBehaviour
{
    [Header("Fuzzy Settings")]
    public AnimationCurve distanceToSpeed;
    public AnimationCurve healthToAggressiveness;
    public AnimationCurve hungerToSpeed;
    public float maxDistance = 60f;
    public float minSpeedMultiplier = 0.6f;
    public float maxSpeedMultiplier = 1.8f;

    [Header("Input Values")]
    public float currentDistance = 0f;
    public float currentHealth = 100f;
    public float currentHunger = 0f;

    [Header("Output Values")]
    public float speedMultiplier = 1f;
    public float aggressivenessMultiplier = 1f;

    private HealthBase health;
    private Blackboard blackboard;

    private void Reset()
    {
        InitializeDefaultCurves();
    }

    private void Start()
    {
        InitializeDefaultCurves();
        health = GetComponent<HealthBase>();
        blackboard = GetComponent<Blackboard>();
    }

    private void Update()
    {
        if (blackboard != null)
        {
            currentHunger = blackboard.GetFloat("Hunger", 0f);
            currentHealth = health != null ? health.HealthPercentage * 100f : 100f;

            AIEyeBase eyeBase = GetComponent<AIEyeBase>();
            if (eyeBase != null)
            {
                Transform target = eyeBase.GetNearestTarget();
                currentDistance = target != null
                    ? Vector3.Distance(transform.position, target.position)
                    : 20f;
            }
        }

        speedMultiplier = EvaluateSpeedByDistance(currentDistance);
        aggressivenessMultiplier = Mathf.Clamp01(healthToAggressiveness.Evaluate(Mathf.Clamp01(currentHealth / 100f)));
    }

    public float EvaluateSpeedByDistance(float distance)
    {
        currentDistance = distance;
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        float fuzzyValue = Mathf.Clamp01(distanceToSpeed.Evaluate(normalizedDistance));
        speedMultiplier = Mathf.Lerp(minSpeedMultiplier, maxSpeedMultiplier, fuzzyValue);
        return speedMultiplier;
    }

    public float GetSpeedMultiplier() => speedMultiplier;
    public float GetAggressivenessMultiplier() => aggressivenessMultiplier;

    private void InitializeDefaultCurves()
    {
        if (IsCurveEmpty(distanceToSpeed))
        {
            distanceToSpeed = CreateSmoothCurve(
                new Keyframe(0f, 1f),
                new Keyframe(0.5f, 0.6f),
                new Keyframe(1f, 0.2f));
        }

        if (IsCurveEmpty(healthToAggressiveness))
        {
            healthToAggressiveness = CreateSmoothCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.5f, 0.6f),
                new Keyframe(1f, 1f));
        }

        if (IsCurveEmpty(hungerToSpeed))
        {
            hungerToSpeed = CreateSmoothCurve(
                new Keyframe(0f, 0.4f),
                new Keyframe(0.5f, 0.7f),
                new Keyframe(1f, 1f));
        }
    }

    private bool IsCurveEmpty(AnimationCurve curve)
    {
        return curve == null || curve.length == 0;
    }

    private AnimationCurve CreateSmoothCurve(params Keyframe[] keys)
    {
        AnimationCurve curve = new AnimationCurve(keys);

        for (int i = 0; i < curve.length; i++)
            curve.SmoothTangents(i, 0f);

        return curve;
    }
}
