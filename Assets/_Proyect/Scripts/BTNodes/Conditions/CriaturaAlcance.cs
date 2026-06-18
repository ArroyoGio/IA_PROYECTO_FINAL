using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CriaturaAlcance : Conditional
{
    public SharedTransform target;
    public float range = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;

        float distance = Vector3.Distance(transform.position, target.Value.position);
        return distance < range ? TaskStatus.Success : TaskStatus.Failure;
    }
}