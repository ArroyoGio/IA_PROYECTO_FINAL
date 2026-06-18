using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RayoElectrico : Action
{
    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            kraken.RayoElectrico();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}