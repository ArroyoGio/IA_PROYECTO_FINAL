using UnityEngine;

public abstract class PredatorVehicleLand : AICharacterVehicle
{
     
    public override void LoadComponent()
    {
        base.LoadComponent();

    }

    protected void EvadeEnemy()
    { 
       if(eye.ViewEnemy != null)
       {
           // Lógica para evadir al enemigo
           EvadeBehaviour(eye.ViewEnemy.transform,transform.forward*10f);
       }
    }
    protected void SeguirEnemy()
    {
        if (eye.ViewEnemy != null)
        {
            // Lógica para seguir al enemigo
            ArriveBehaviour(eye.ViewEnemy.transform.position, 2f);
        }
    }
    protected void Patrullar()
            {
        // Lógica para patrullar el área
        WanderBehaviour();
    }
}