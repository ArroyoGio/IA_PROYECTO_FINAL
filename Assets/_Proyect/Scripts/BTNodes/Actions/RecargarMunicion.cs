using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RecargarMunicion : Action
{
    public override TaskStatus OnUpdate()
    {
        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.RecargarMunicion();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}