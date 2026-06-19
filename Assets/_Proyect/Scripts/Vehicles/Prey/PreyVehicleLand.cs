using UnityEngine;

public abstract class PreyVehicleLand : AICharacterVehicle
{
    public override void LoadComponent()
    {
        base.LoadComponent();

    }

    protected void EvadeEnemy()
    {
        if (eye.ViewEnemy != null)
        {
            // Lógica para evadir al enemigo
            EvadeBehaviour(eye.ViewEnemy.transform, transform.forward * 10f);
        }
    }
    protected void Patrullar()
    {
        // Lógica para patrullar el área
        WanderBehaviour();
    }

}