using BehaviorDesigner.Runtime.Tasks;

public class BTWander : Action
{
    private PredatorVehicleLand predator;
    private PreyVehicleLand prey;
    private SurvivorVehicleLand survivor;

    public override void OnAwake()
    {
        predator = GetComponent<PredatorVehicleLand>();
        prey = GetComponent<PreyVehicleLand>();
        survivor = GetComponent<SurvivorVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (predator != null)
        {
            predator.Patrullar();
            return TaskStatus.Success;
        }

        if (prey != null)
        {
            prey.Patrullar();
            return TaskStatus.Success;
        }

        if (survivor != null)
        {
            survivor.Patrullar();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
