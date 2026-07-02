using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class HealingCoralItem : MonoBehaviour
{
    public float healAmount = 25f;

    private bool consumed;

    private void Awake()
    {
        Collider itemCollider = GetComponent<Collider>();
        itemCollider.isTrigger = true;

        Rigidbody itemRigidbody = GetComponent<Rigidbody>();
        itemRigidbody.isKinematic = true;
        itemRigidbody.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryConsume(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryConsume(other);
    }

    private void TryConsume(Collider other)
    {
        if (consumed) return;

        HealthBase health = other.GetComponentInParent<HealthBase>();
        if (health == null || health.IsDead)
            return;

        consumed = true;
        health.Heal(healAmount);
        Destroy(gameObject);
    }
}
