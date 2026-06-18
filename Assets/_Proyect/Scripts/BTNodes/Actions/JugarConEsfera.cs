using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class JugarConEsfera : Action
{
    public override TaskStatus OnUpdate()
    {
        DolphinActionLand dolphin = GetComponent<DolphinActionLand>();
        if (dolphin != null)
        {
            dolphin.JugarConEsfera();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}