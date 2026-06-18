using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Explorar : Action
{
    public override TaskStatus OnUpdate()
    {
        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.Explorar();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}