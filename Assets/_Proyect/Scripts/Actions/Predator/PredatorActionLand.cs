using UnityEngine;

public abstract class PredatorActionLand : AICharacterAction
{
    [Header("Predator Settings")]
    public float attackRange = 10f;
    public float attackDamage = 25f;
    public float aggressiveness = 50f;
    public float huntingRange = 20f;
    public WeaponType weaponType;

    protected void BlackboardPresaAlcance()
    {
        bool presaAlcance = eye != null &&
                            eye.ViewEnemy != null &&
                            IsTargetInRange(eye.ViewEnemy.transform);

        if (blackboard != null)
            blackboard.SetBool("PresaAlcance", presaAlcance);
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        BlackboardPresaAlcance();
    }

    public virtual void Attack(Transform target)
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackRange)
            return;

        HealthBase health = target.GetComponentInParent<HealthBase>();

        if (health != null)
        {
            health.ApplyDamage(attackDamage, weaponType);
        }
    }
    public virtual bool IsTargetInRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }
}