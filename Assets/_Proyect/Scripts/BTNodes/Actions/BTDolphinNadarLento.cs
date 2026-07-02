using BehaviorDesigner.Runtime.Tasks;

public class BTDolphinNadarLento : Action
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

        dolphin.NadarLento();
        return dolphin.energy > dolphin.UpEnergiaBaja + 10f
            ? TaskStatus.Success
            : TaskStatus.Running;
    }
}
