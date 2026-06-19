using UnityEngine;
using UnityEngine.AI;

public class HealthCombat : Health
{
    [Header("Combat Settings")]
    public float armor = 5f;
    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;
    public AudioClip deathSound;
 

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void TakeDamage(float damage)
    {
        if (IsDead) return;

        float finalDamage = Mathf.Max(0f, damage - armor);

        base.TakeDamage(finalDamage);

        if (hitParticles != null)
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
        }

        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            blackboard.SetBool("IsHurt", true);
        }
    }

    public override void Die()
    {
        base.Die();
 
        if (deathParticles != null)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        gameObject.SetActive(false);
    }
}