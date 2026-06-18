using UnityEngine;

    public class HealthCombat : HealthIA
    {
        [Header("Combat Settings")]
        public ParticleSystem deathParticles;
        public GameObject ragdollPrefab;
        public float respawnTime = 5f;
        public Transform respawnPoint;

        public override void ApplyDamage(float damage, WeaponType type)
        {
            SetHurtingMe(GetLastAttacker());
            base.ApplyDamage(damage, type);

            // Activar modo defensivo si está en peligro
            if (IsDangerHealth)
            {
                var blackboard = GetComponent<Blackboard>();
                if (blackboard != null)
                {
                    blackboard.SetBool("IsDanger", true);
                }
            }
        }

        GameObject GetLastAttacker()
        {
            // Buscar el último atacante en el blackboard
            var blackboard = GetComponent<Blackboard>();
            if (blackboard != null)
            {
                return blackboard.GetObject("LastAttacker") as GameObject;
            }
            return null;
        }

        public override void Death()
        {
            base.Death();

            // Desactivar NavMeshAgent
            var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            // Partículas de muerte
            if (deathParticles != null)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
            }

            // Ragdoll
            if (ragdollPrefab != null)
            {
                Instantiate(ragdollPrefab, transform.position, transform.rotation);
            }

            // Desactivar el NPC
            gameObject.SetActive(false);

            // Notificar al Behavior Tree
            var blackboard = GetComponent<Blackboard>();
            if (blackboard != null)
            {
                blackboard.SetBool("IsDead", true);
            }

            // Programar respawn
            Invoke(nameof(Respawn), respawnTime);
        }

        public virtual void Respawn()
        {
            Vector3 spawnPosition = respawnPoint != null ? respawnPoint.position : Vector3.zero;

            gameObject.transform.position = spawnPosition;

            var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = true;

            Active();

            var blackboard = GetComponent<Blackboard>();
            if (blackboard != null)
            {
                blackboard.SetBool("IsDead", false);
                blackboard.SetBool("IsDanger", false);
            }
        }

        public override void Active()
        {
            base.Active();
            var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = true;
        }
    }