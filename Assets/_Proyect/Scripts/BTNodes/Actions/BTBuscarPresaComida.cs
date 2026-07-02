using BehaviorDesigner.Runtime.Tasks;

public class BTBuscarPresaComida : Action
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

        if (!octopus.HasPreyFoodNearby())
            return TaskStatus.Failure;

        octopus.BuscarPresaComida();

        return octopus.HasPreyFoodNearby()
            ? TaskStatus.Running
            : TaskStatus.Success;
    }
}
