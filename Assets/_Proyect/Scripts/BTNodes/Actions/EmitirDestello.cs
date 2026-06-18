using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EmitirDestello : Action
{
    public override TaskStatus OnUpdate()
    {
        SnakeActionLand snake = GetComponent<SnakeActionLand>();
        if (snake != null)
        {
            snake.EmitirDestello();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}