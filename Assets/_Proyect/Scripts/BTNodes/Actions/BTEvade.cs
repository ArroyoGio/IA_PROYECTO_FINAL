using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTEvade : Action
{
    private PreyVehicleLand preyVehicle;

    public override void OnAwake()
    {
        preyVehicle = GetComponent<PreyVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (preyVehicle == null)
            return TaskStatus.Failure;

        preyVehicle.EvadeEnemy();
        return TaskStatus.Success;
    }
}
