using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CrearTorbellino : Action
{
    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.CrearTorbellino();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}