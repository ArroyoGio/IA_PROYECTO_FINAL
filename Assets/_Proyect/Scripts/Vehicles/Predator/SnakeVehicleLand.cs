using UnityEngine;

public class SnakeVehicleLand : PredatorVehicleLand
{
    private float normalMaxSpeed;
    private bool hasNormalMaxSpeed;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        CacheNormalSpeed();
    }

    public new void Patrullar()
    {
        MoveStealth();
        WanderBehaviour();
    }

    public new void SeguirPrey()
    {
        RestoreNormalSpeed();
        base.SeguirPrey();
    }

    public void MoveStealth()
    {
        CacheNormalSpeed();
        maxSpeed = normalMaxSpeed * 0.45f;
    }

    public void RestoreNormalSpeed()
    {
        if (hasNormalMaxSpeed)
            maxSpeed = normalMaxSpeed;
    }

    private void CacheNormalSpeed()
    {
        if (hasNormalMaxSpeed)
            return;

        normalMaxSpeed = maxSpeed;
        hasNormalMaxSpeed = true;
    }
}
