using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AlienFoodItem : MonoBehaviour
{
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

        FishActionLand fish = other.GetComponentInParent<FishActionLand>();
        if (fish != null)
        {
            consumed = true;
            fish.Comer();
            Destroy(gameObject);
            return;
        }

        OctopusActionLand octopus = other.GetComponentInParent<OctopusActionLand>();
        if (octopus != null)
        {
            consumed = true;
            octopus.Comer();
            Destroy(gameObject);
        }
    }
}
