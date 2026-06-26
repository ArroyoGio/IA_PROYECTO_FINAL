using BehaviorDesigner.Runtime.Tasks;

public class ConditionEnergiaBaja : Conditional
{
    private AICharacterAction action;

    public override void OnAwake()
    {
        action = GetComponent<AICharacterAction>();
    }

    public override TaskStatus OnUpdate()
    {
        if (action == null)
            return TaskStatus.Failure;

        return action.energy < action.UpEnergiaBaja
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
