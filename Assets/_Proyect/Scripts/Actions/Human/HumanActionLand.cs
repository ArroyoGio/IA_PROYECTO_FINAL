using UnityEngine;

public abstract class HumanActionLand : AICharacterAction
{
    protected DiverVehicleLand diverVehicle;

    public override void LoadComponent()
    {
        base.LoadComponent();
        diverVehicle = GetComponent<DiverVehicleLand>();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
    }
}
