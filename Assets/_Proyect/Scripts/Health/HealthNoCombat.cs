using UnityEngine;
using UnityEngine.AI;

public class HealthNoCombat : Health
{
    [Header("No Combat Settings")]
    public float regenerationRate = 0.5f;
    public bool autoRegenerate = true;

   

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected virtual void Update()
    {
        if (!autoRegenerate || IsDead) return;

        Heal(regenerationRate * Time.deltaTime);
    }

    public override void Die()
    {
        base.Die();

        
        gameObject.SetActive(false);
    }
}