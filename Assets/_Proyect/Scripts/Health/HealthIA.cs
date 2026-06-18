using UnityEngine;
    public class HealthIA : HealthBase
    {
        [Header("IA References")]
        public AICharacterControl aiControl;
        public AIEyeBase eye;

        public GameObject HurtingMe { get; protected set; }
        public float lastDamageTime { get; protected set; }

        public override void Awake()
        {
            base.Awake();
            aiControl = GetComponent<AICharacterControl>();
            eye = GetComponent<AIEyeBase>();
        }

        public override void ApplyDamage(float damage, WeaponType type)
        {
            base.ApplyDamage(damage, type);
            lastDamageTime = Time.time;
        }

        public virtual void SetHurtingMe(GameObject attacker)
        {
            HurtingMe = attacker;
            // Limpiar después de 4 segundos
            Invoke(nameof(ClearHurtingMe), 4f);
        }

        void ClearHurtingMe()
        {
            HurtingMe = null;
        }

        public override void Active()
        {
            base.Active();
            HurtingMe = null;
            if (aiControl != null)
            {
                aiControl.enabled = true;
            }
        }

        public override void Death()
        {
            base.Death();
            if (aiControl != null)
            {
                aiControl.enabled = false;
            }
        }
    }