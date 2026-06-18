using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EmitirPulsoNode : Action
{
    public override TaskStatus OnUpdate()
    {
        SphereActionLand sphere = GetComponent<SphereActionLand>();
        if (sphere != null)
        {
            sphere.EmitirPulso();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}