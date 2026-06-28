using UnityEngine;

public abstract class SurvivorVehicleLand : AICharacterVehicle
{
    protected float normalMaxSpeed;
    protected bool hasNormalMaxSpeed;

    public override void LoadComponent()
    {
        base.LoadComponent();
        CacheNormalSpeed();
    }

    public virtual void Patrullar()
    {
        CacheNormalSpeed();
        WanderBehaviour();
    }

    public virtual void SetSpeedMultiplier(float multiplier)
    {
        CacheNormalSpeed();
        maxSpeed = normalMaxSpeed * multiplier;
    }

    public virtual void CambiarPosicion()
    {
        Patrullar();
    }

    public virtual void EvadeEnemy()
    {
        if (eye == null || eye.ViewEnemy == null)
            return;

        Vector3 enemyVelocity = Vector3.zero;
        AICharacterVehicle enemyVehicle = eye.ViewEnemy.GetComponent<AICharacterVehicle>();

        if (enemyVehicle != null)
            enemyVelocity = enemyVehicle.GetVelocity();

        EvadeBehaviour(eye.ViewEnemy.transform, enemyVelocity);
    }

    public virtual void RestoreNormalSpeed()
    {
        if (hasNormalMaxSpeed)
            maxSpeed = normalMaxSpeed;
    }

    protected void CacheNormalSpeed()
    {
        if (hasNormalMaxSpeed)
            return;

        normalMaxSpeed = maxSpeed;
        hasNormalMaxSpeed = true;
    }
}
