using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Dormir : Action
{
    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.Dormir();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}