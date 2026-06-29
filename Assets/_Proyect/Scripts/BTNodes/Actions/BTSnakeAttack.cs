using BehaviorDesigner.Runtime.Tasks;

public class BTSnakeAttack : Action
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

        snake.Attack();
        return TaskStatus.Success;
    }
}
