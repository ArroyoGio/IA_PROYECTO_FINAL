using UnityEngine;

public abstract class PredatorActionLand : AICharacterAction
{
    [Header("Predator Settings")]
    public float attackRange = 5f;
    [Header("Predator damage")]
    public float attackDamage = 25f;
    [Header("Predator Settings")]
 
    public float aggressiveness = 50f;
    public float huntingRange = 20f;
    [Header("Predator WeaponType")]
    public WeaponType weaponType;
    public override void LoadComponent()
    {
        base.LoadComponent();

    }
    protected void BlackboardPresaAlcance()
    {
        if (blackboard != null)
        {
            blackboard.SetBool("PresaAlcance", IsTargetInRange(eye.ViewEnemy?.transform));
        }
    }
    public override void UpdateAI()
    {
       base.UpdateAI();
       BlackboardPresaAlcance();
    }
     
    public virtual void Attack(Transform target)
    {
        if (target != null && IsTargetInRange(target))
        {
            HealthBase health = target.GetComponent<HealthBase>();
            if (health != null)
            {
                health.ApplyDamage(attackDamage, weaponType);
            }
        }
    }

    public virtual bool IsTargetInRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < attackRange;
    }
}