using BehaviorDesigner.Runtime.Tasks;

public class ConditionCuriosidadAlta : Conditional
{
    private DolphinActionLand dolphin;

    public override void OnAwake()
    {
        dolphin = GetComponent<DolphinActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (dolphin == null)
            return TaskStatus.Failure;

        return dolphin.curiosity >= dolphin.curiosityHighThreshold
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
