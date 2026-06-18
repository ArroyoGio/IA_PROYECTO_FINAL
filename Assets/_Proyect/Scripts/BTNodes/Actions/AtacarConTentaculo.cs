using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AtacarConTentaculo : Action
{
    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.AtacarConTentaculo();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}