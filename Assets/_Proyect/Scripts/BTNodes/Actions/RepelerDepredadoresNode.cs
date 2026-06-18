using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RepelerDepredadoresNode : Action
{
    public override TaskStatus OnUpdate()
    {
        SphereActionLand sphere = GetComponent<SphereActionLand>();
        if (sphere != null)
        {
            sphere.RepelerDepredadores();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}