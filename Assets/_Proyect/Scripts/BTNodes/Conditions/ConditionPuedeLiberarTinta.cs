using BehaviorDesigner.Runtime.Tasks;

public class ConditionPuedeLiberarTinta : Conditional
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

        return octopus.PuedeLiberarTinta() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
