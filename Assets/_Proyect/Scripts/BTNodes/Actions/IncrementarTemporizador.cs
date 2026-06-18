using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IncrementarTemporizador : Action
{
    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.IncrementarTemporizador();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}