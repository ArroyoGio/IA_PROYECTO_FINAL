using UnityEngine;

public class OctopusVehicleLand : SurvivorVehicleLand
{
    [Header("Octopus Vehicle")]
    public float camouflageSpeed = 0.5f;
    public float inkDispersionForce = 5f;

    private Renderer renderer;

    protected override void Awake()
    {
        base.Awake();
        renderer = GetComponent<Renderer>();
    }

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void CamouflageMove(Vector3 target)
    {
        float originalSpeed = maxSpeed;
        maxSpeed = camouflageSpeed;

        // Reducir visibilidad
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = 0.2f;
            renderer.material.color = color;
        }

        Move(target);
        maxSpeed = originalSpeed;
    }

    public void RestoreVisibility()
    {
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = 1f;
            renderer.material.color = color;
        }
    }

    public void DisperseInk(Vector3 direction)
    {
        if (rb != null)
        {
            rb.AddForce(direction * inkDispersionForce, ForceMode.Impulse);
        }
    }
}