using BehaviorDesigner.Runtime.Tasks;

public class BTCamuflarse : Action
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

        octopus.Camuflarse();
        return TaskStatus.Success;
    }
}
