using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WanderNode : Action
{
    public override TaskStatus OnUpdate()
    {
        Wander wander = GetComponent<Wander>();
        if (wander != null)
        {
            wander.isActive = true;
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}