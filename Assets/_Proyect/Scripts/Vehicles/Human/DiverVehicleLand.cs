using UnityEngine;

public class DiverVehicleLand : HumanVehicleLand
{
    [Header("Diver Vehicle")]
    public float ascentSpeed = 2f;
    public float descentSpeed = 1.5f;
    public float huntSpeed = 4f;

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void Ascend(Vector3 surfacePoint)
    {
        float originalSpeed = maxSpeed;
        maxSpeed = ascentSpeed;
        Move(surfacePoint);
        maxSpeed = originalSpeed;
    }

    public void Descend(Vector3 depthPoint)
    {
        float originalSpeed = maxSpeed;
        maxSpeed = descentSpeed;
        Move(depthPoint);
        maxSpeed = originalSpeed;
    }

    public void HuntMove(Transform prey)
    {
        if (prey != null)
        {
            float originalSpeed = maxSpeed;
            maxSpeed = huntSpeed;
            Move(prey.position);
            maxSpeed = originalSpeed;
        }
    }

    public void Explore(Vector3 explorePoint)
    {
        Move(explorePoint);
    }
}