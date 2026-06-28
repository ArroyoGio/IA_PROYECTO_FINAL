using BehaviorDesigner.Runtime.Tasks;

public class ConditionAmenazaAlta : Conditional
{
    public float threshold = 60f;

    private LobsterActionLand lobster;

    public override void OnAwake()
    {
        lobster = GetComponent<LobsterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (lobster == null)
            return TaskStatus.Failure;

        return lobster.threat > threshold && lobster.HasPredatorThreat()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
