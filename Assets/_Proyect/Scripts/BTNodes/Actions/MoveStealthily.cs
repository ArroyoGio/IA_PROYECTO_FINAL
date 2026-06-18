using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveStealthily : Action
{
    public override TaskStatus OnUpdate()
    {
        SnakeActionLand snake = GetComponent<SnakeActionLand>();
        if (snake != null)
        {
            snake.MoveStealthily();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}