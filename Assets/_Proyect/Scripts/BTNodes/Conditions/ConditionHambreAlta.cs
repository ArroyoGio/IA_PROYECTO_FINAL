using BehaviorDesigner.Runtime.Tasks;

public class ConditionHambreAlta : Conditional
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

        return action.hunger > action.UpHambreAlta
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
