using BehaviorDesigner.Runtime.Tasks;

public class ConditionPuedeEmitirDestello : Conditional
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

        return snake.CanFlash()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
