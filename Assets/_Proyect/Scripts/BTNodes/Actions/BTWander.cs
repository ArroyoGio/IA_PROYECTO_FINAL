using BehaviorDesigner.Runtime.Tasks;

public class BTWander : Action
{
    private PredatorVehicleLand predator;
    private PreyVehicleLand prey;

    public override void OnAwake()
    {
        predator = GetComponent<PredatorVehicleLand>();
        prey = GetComponent<PreyVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (predator != null)
        {
            predator.Patrullar();
            return TaskStatus.Running;
        }

        if (prey != null)
        {
            prey.Patrullar();
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}