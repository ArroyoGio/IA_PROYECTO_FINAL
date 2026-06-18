using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class VeAmenaza : Conditional
{
    public SharedTransform threatTarget;
    public LayerMask threatLayer;

    public override TaskStatus OnUpdate()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, 15f, threatLayer);
        if (threats.Length > 0)
        {
            threatTarget.Value = threats[0].transform;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}