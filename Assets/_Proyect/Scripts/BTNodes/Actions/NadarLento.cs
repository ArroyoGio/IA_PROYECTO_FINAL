using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NadarLento : Action
{
    public override TaskStatus OnUpdate()
    {
        DolphinActionLand dolphin = GetComponent<DolphinActionLand>();
        if (dolphin != null)
        {
            dolphin.NadarLento();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}