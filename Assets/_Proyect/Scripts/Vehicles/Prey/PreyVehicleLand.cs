using UnityEngine;

public abstract class PreyVehicleLand : AICharacterVehicle
{
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    protected bool SeeEnemy()
    {
        return eye != null && eye.ViewEnemy != null;
    }

    public void EvadeEnemy()
    {
        if (!SeeEnemy()) return;

        Vector3 enemyVelocity = Vector3.zero;

        AICharacterVehicle enemyVehicle =
            eye.ViewEnemy.GetComponent<AICharacterVehicle>();

        if (enemyVehicle != null)
            enemyVelocity = enemyVehicle.GetVelocity();

        EvadeBehaviour(eye.ViewEnemy.transform, enemyVelocity);
    }

    public void Patrullar()
    {
        WanderBehaviour();
    }
}