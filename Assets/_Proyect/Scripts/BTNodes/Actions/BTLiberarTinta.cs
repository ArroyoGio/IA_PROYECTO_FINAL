using BehaviorDesigner.Runtime.Tasks;

public class BTLiberarTinta : Action
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

        if (!octopus.PuedeLiberarTinta())
            return TaskStatus.Failure;

        octopus.LiberarTinta();
        return TaskStatus.Success;
    }
}
