using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Enterrarse : Action
{
    public override TaskStatus OnUpdate()
    {
        LobsterActionLand lobster = GetComponent<LobsterActionLand>();
        if (lobster != null)
        {
            lobster.Enterrarse();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}