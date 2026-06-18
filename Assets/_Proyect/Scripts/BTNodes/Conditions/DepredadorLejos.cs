using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DepredadorLejos : Conditional
{
    public float safeDistance = 20f;

    public override TaskStatus OnUpdate()
    {
        AIEyeBase eye = GetComponent<AIEyeBase>();
        if (eye != null)
        {
            Transform threat = eye.GetNearestTarget();
            if (threat != null)
            {
                float distance = Vector3.Distance(transform.position, threat.position);
                return distance > safeDistance ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}