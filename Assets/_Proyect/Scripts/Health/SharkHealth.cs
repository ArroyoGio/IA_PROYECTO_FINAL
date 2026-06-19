using UnityEngine;

public class SharkHealth : HealthCombat
{
    [Header("Shark Settings")]
    public float huntingThreshold = 50f;

    public bool CanHunt => CurrentHealth > huntingThreshold;

    protected override void Awake()
    {
        base.Awake();
        armor = 10f;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            blackboard.SetBool("CanHunt", CanHunt);
        }
    }
}