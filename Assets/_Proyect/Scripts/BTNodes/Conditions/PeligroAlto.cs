using BehaviorDesigner.Runtime.Tasks;

public class PeligroAlto : Conditional
{
    private DiverActionLand diver;

    public override void OnAwake()
    {
        diver = GetComponent<DiverActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (diver == null)
            return TaskStatus.Failure;

        return diver.HasHighDanger()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
