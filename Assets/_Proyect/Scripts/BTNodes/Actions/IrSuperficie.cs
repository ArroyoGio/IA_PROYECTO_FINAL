using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IrSuperficie : Action
{
    public override TaskStatus OnUpdate()
    {
        DolphinActionLand dolphin = GetComponent<DolphinActionLand>();
        if (dolphin != null)
        {
            dolphin.IrSuperficie();
            return TaskStatus.Running;
        }

        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.GoToSurface();
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}