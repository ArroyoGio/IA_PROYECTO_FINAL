using BehaviorDesigner.Runtime.Tasks;

public class ConditionPresaComidaCerca : Conditional
{
    private OctopusActionLand octopus;

    public override void OnAwake()
    {
        octopus = GetComponent<OctopusActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (octopus == null)
            return TaskStatus.Failure;

        return octopus.HasPreyFoodNearby()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
