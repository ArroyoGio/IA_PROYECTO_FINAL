using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class VeCriatura : Conditional
{
    public SharedTransform detectedTarget;
    public LayerMask creatureLayer;

    public override TaskStatus OnUpdate()
    {
        Collider[] creatures = Physics.OverlapSphere(transform.position, 20f, creatureLayer);
        if (creatures.Length > 0)
        {
            detectedTarget.Value = creatures[0].transform;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}