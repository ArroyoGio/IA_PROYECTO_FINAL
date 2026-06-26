using BehaviorDesigner.Runtime.Tasks;

public class ConditionCamuflajeDisponible : Conditional
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

        return octopus.camouflage > 0f ? TaskStatus.Success : TaskStatus.Failure;
    }
}
