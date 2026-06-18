using UnityEngine;

public abstract class PreyActionLand : AICharacterAction
{
    [Header("Prey Settings")]
    public float fear = 0f;
    public float maxFear = 100f;

    public virtual void Evade()
    {
        var evade = GetComponent<Evade>();
        if (evade != null)
        {
            Transform threat = eye.GetNearestTarget();
            if (threat != null)
            {
                evade.SetThreat(threat);
                evade.isActive = true;
                steering.AddBehavior(evade);
            }
        }
    }

    public virtual void JoinSchool()
    {
        Collider[] fish = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Fish"));
        if (fish.Length > 1)
        {
            Vector3 center = Vector3.zero;
            foreach (var f in fish)
            {
                center += f.transform.position;
            }
            center /= fish.Length;
            steering.SetTarget(center);
        }
    }

    public virtual void Rest() { }
    public virtual void FindFood() { }
}