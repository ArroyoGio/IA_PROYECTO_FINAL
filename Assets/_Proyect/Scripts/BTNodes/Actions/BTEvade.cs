using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTEvade : Action
{
    private PreyVehicleLand preyVehicle;
    private SurvivorVehicleLand survivorVehicle;

    public override void OnAwake()
    {
        preyVehicle = GetComponent<PreyVehicleLand>();
        survivorVehicle = GetComponent<SurvivorVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (preyVehicle != null)
        {
            preyVehicle.EvadeEnemy();
            return TaskStatus.Success;
        }

        if (survivorVehicle != null)
        {
            survivorVehicle.EvadeEnemy();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
