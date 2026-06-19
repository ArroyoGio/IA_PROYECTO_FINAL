using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    
    protected float currentHealth;

    [Header("Events")]
    public Action<float> OnDamageTaken;
    public Action OnDeath;
    public Action OnHeal;

    public float CurrentHealth => currentHealth;
    public float HealthPercentage => currentHealth / maxHealth;
    public bool IsDead => currentHealth <= 0f;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnDamageTaken?.Invoke(damage);

        if (IsDead)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHeal?.Invoke();
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
    }

    public virtual void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}