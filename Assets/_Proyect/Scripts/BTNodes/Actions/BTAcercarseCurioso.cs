using BehaviorDesigner.Runtime.Tasks;

public class BTAcercarseCurioso : Action
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

        if (!dolphin.HasCuriosityTarget())
            return TaskStatus.Failure;

        dolphin.AcercarseCurioso();

        return dolphin.IsCuriosityTargetReached()
            ? TaskStatus.Success
            : TaskStatus.Running;
    }
}
