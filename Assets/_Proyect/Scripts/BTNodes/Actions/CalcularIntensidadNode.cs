using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CalcularIntensidadNode : Action
{
    public override TaskStatus OnUpdate()
    {
        SphereActionLand sphere = GetComponent<SphereActionLand>();
        if (sphere != null)
        {
            sphere.CalcularIntensidad();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}