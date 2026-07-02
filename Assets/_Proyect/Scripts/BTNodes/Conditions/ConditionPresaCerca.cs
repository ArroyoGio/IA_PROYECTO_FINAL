using BehaviorDesigner.Runtime.Tasks;

public class ConditionPresaCerca : Conditional
{
    private LobsterActionLand lobster;

    public override void OnAwake()
    {
        lobster = GetComponent<LobsterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (lobster == null)
            return TaskStatus.Failure;

        bool preyClose = lobster.IsPreyClose();

        return preyClose
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
