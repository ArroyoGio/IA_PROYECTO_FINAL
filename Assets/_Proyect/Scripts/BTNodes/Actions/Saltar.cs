using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Saltar : Action
{
    public override TaskStatus OnUpdate()
    {
        DolphinActionLand dolphin = GetComponent<DolphinActionLand>();
        if (dolphin != null)
        {
            dolphin.Saltar();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}