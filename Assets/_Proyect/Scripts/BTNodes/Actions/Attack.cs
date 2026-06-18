using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AttackNode : Action
{
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;

        SharkActionLand shark = GetComponent<SharkActionLand>();
        if (shark != null)
        {
            shark.Morder();
            return TaskStatus.Success;
        }

        SnakeActionLand snake = GetComponent<SnakeActionLand>();
        if (snake != null)
        {
            snake.Constreñir();
            return TaskStatus.Success;
        }

        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.AtacarConTentaculo();
            return TaskStatus.Success;
        }

        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.Disparar();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}