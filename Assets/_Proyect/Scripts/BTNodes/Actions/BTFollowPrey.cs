using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTFollowPrey : Action
{
    private PredatorVehicleLand predatorVehicle;

    public override void OnAwake()
    {
        predatorVehicle = GetComponent<PredatorVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("BTFollowPrey ejecutándose");

        if (predatorVehicle == null)
        {
            Debug.Log("No encontró PredatorVehicleLand");
            return TaskStatus.Failure;
        }

        predatorVehicle.SeguirPrey();
        return TaskStatus.Success;
    }
}
