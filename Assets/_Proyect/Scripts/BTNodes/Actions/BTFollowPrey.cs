using BehaviorDesigner.Runtime.Tasks;

public class BTFollowPrey : Action
{
    private PredatorVehicleLand predatorVehicle;

    public override void OnAwake()
    {
        predatorVehicle = GetComponent<PredatorVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (predatorVehicle == null)
            return TaskStatus.Failure;

        predatorVehicle.SeguirPrey();
        return TaskStatus.Success;
    }
}
