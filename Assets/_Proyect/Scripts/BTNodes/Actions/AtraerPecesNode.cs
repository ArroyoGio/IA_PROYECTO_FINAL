using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AtraerPecesNode : Action
{
    public override TaskStatus OnUpdate()
    {
        SphereActionLand sphere = GetComponent<SphereActionLand>();
        if (sphere != null)
        {
            sphere.AtraerPeces();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}