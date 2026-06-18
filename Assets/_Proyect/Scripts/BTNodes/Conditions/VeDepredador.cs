using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class VeDepredador : Conditional
{
    public SharedTransform detectedTarget;

    public override TaskStatus OnUpdate()
    {
        AIEyeBase eye = GetComponent<AIEyeBase>();
        if (eye != null && eye.HasTargets())
        {
            detectedTarget.Value = eye.GetNearestTarget();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}