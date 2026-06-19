using UnityEngine;
using UnityEngine.AI;

public class DiverHealth : HealthCombat
{
    [Header("Diver Settings")]
    public float oxygenConsumption = 0.1f;
    public float maxOxygen = 100f;

    protected float currentOxygen;

    protected override void Awake()
    {
        base.Awake();
        currentOxygen = maxOxygen;
    }

    protected virtual void Update()
    {
        if (IsDead) return;

        currentOxygen -= oxygenConsumption * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        if (currentOxygen <= 0f)
        {
            TakeDamage(1f);
        }

        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            blackboard.SetFloat("Oxygen", currentOxygen);
        }
    }

    public void RestoreOxygen(float amount)
    {
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
    }
}