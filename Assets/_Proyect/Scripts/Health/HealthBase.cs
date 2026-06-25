using UnityEngine;
using System;

  public enum WeaponType
    {
        Normal,
        Explosive,
        Fire,
        Electric,
        Poison,
        Tentacle,
        Whirlpool,
        Lightning
    }

    public class HealthBase : MonoBehaviour
    {
        [Header("Health Settings")]
        public float maxHealth = 100f;
        public float health = 100f;
        public float armor = 0f;
        public bool isImmortal = false;

        [Header("Events")]
        public Action OnDeath;
        public Action<float, WeaponType> OnDamageReceived;
        public Action OnHeal;

        public bool IsDead => health <= 0;
        public bool IsDangerHealth => health < maxHealth * 0.5f;
        public float HealthPercentage => health / maxHealth;

        public virtual void Awake()
        {
            health = maxHealth;
        }

    public virtual void ApplyDamage(float damage, WeaponType type = WeaponType.Normal)
    {
        Debug.Log(gameObject.name + " recibió daño: " + damage);

        if (isImmortal || IsDead) return;

        float finalDamage = Mathf.Max(0, damage - armor);
        health -= finalDamage;
        health = Mathf.Clamp(health, 0, maxHealth);

        OnDamageReceived?.Invoke(finalDamage, type);

        if (IsDead)
        {
            Death();
        }
    }

    public virtual void Heal(float amount)
        {
            if (IsDead) return;
            health = Mathf.Clamp(health + amount, 0, maxHealth);
            OnHeal?.Invoke();
        }

        public virtual void Death()
        {
            OnDeath?.Invoke();
        }

        public virtual void Active()
        {
            health = maxHealth;
            gameObject.SetActive(true);
        }

        public virtual void Desactive()
        {
            gameObject.SetActive(false);
        }

        public virtual void ResetHealth()
        {
            health = maxHealth;
        }
    }
