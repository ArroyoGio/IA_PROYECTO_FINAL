using UnityEngine;

public abstract class PredatorActionLand : AICharacterAction
{
    [Header("Predator Settings")]
    public float attackRange = 5f;

    public virtual void Pursue(Transform target)
    {
        var pursuit = GetComponent<Pursuit>();
        if (pursuit != null && target != null)
        {
            pursuit.SetTarget(target);
            pursuit.isActive = true;
            steering.AddBehavior(pursuit);
        }
    }

    public virtual void Attack(Transform target)
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            HealthBase health = target.GetComponent<HealthBase>();
            if (health != null)
            {
                health.ApplyDamage(25f, WeaponType.Normal);
            }
        }
    }

    public virtual bool IsTargetInRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < attackRange;
    }
}