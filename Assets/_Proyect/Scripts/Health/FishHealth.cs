using UnityEngine;

public class FishHealth : HealthNoCombat
{
    [Header("Fish Settings")]
    public float stressThreshold = 30f;

    public bool IsStressed => CurrentHealth < stressThreshold;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            blackboard.SetBool("IsStressed", IsStressed);
        }
    }
}