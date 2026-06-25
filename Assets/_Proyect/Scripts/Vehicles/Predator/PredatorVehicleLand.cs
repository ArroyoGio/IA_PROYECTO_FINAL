using UnityEngine;

public abstract class PredatorVehicleLand : AICharacterVehicle
{
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    /// <summary>
    /// Persigue a la presa utilizando Pursuit.
    /// </summary>
    public void SeguirPrey()
    {
        if (eye == null || eye.ViewEnemy == null)
            return;

        // Se obtiene el vehículo de la presa para conocer su velocidad
        AICharacterVehicle preyVehicle =
            eye.ViewEnemy.GetComponent<AICharacterVehicle>();

        Vector3 preyVelocity =
            preyVehicle != null ?
            preyVehicle.GetVelocity() :
            Vector3.zero;

        // Movimiento de persecución
        PursuitBehaviour(
            eye.ViewEnemy.transform,
            preyVelocity);
    }

    /// <summary>
    /// Patrullaje aleatorio del tiburón.
    /// </summary>
    public void Patrullar()
    {
        WanderBehaviour();
    }
}