using BehaviorDesigner.Runtime.Tasks;

public class ConditionPresaConfundida : Conditional
{
    private SnakeActionLand snake;

    public override void OnAwake()
    {
        snake = GetComponent<SnakeActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (snake == null)
            return TaskStatus.Failure;

        return snake.preyConfused
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
