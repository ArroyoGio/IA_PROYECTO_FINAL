using BehaviorDesigner.Runtime.Tasks;

public class BTEnterrarse : Action
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

        lobster.Enterrarse();
        return TaskStatus.Success;
    }
}
