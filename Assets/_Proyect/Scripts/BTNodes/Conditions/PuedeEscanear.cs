using BehaviorDesigner.Runtime.Tasks;

public class PuedeEscanear : Conditional
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

        return diver.CanScan()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
