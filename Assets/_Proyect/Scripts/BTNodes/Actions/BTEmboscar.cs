using BehaviorDesigner.Runtime.Tasks;

public class BTEmboscar : Action
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

        lobster.Emboscar();
        return TaskStatus.Success;
    }
}
