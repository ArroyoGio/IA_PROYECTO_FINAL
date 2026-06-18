using UnityEngine;

public class HealthNoCombat : HealthIA
{
    [Header("No Combat Settings")]
    public bool isBuilding = false;
    public bool isPickup = false;
    public ParticleSystem deathEffect;

    public override void ApplyDamage(float damage, WeaponType type)
    {
        // Sin armadura, daño directo
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        OnDamageReceived?.Invoke(damage, type);

        if (IsDead)
        {
            Death();
        }
    }

    public override void Death()
    {
        base.Death();

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (isBuilding || isPickup)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        // Notificar al Behavior Tree
        var blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            blackboard.SetBool("IsDead", true);
        }
    }
}