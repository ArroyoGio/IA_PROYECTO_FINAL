using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RecargarBioEnergy : Action
{
    public override TaskStatus OnUpdate()
    {
        SnakeActionLand snake = GetComponent<SnakeActionLand>();
        if (snake != null)
        {
            snake.Recargar();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}