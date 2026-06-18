using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class VePresa : Conditional
{
    public SharedTransform detectedTarget;
    public LayerMask preyLayer;

    public override TaskStatus OnUpdate()
    {
        Collider[] prey = Physics.OverlapSphere(transform.position, 20f, preyLayer);
        if (prey.Length > 0)
        {
            detectedTarget.Value = prey[0].transform;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}