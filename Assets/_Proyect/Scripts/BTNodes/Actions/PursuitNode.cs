using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PursuitNode : Action
{
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;

        PredatorActionLand predator = GetComponent<PredatorActionLand>();
        if (predator != null)
        {
            predator.Pursue(target.Value);
            return TaskStatus.Running;
        }

        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.Pursue(target.Value);
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}