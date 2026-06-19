using UnityEngine;

public class KrakenHealth : HealthCombat
{
    [Header("Kraken Settings")]
    public float regenerationRate = 2f;
    public float regenerationThreshold = 50f;

    protected virtual void Update()
    {
        if (IsDead) return;

        if (CurrentHealth < regenerationThreshold)
        {
            Heal(regenerationRate * Time.deltaTime);
        }
    }
}