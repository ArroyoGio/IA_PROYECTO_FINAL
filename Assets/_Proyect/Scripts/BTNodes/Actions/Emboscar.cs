using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Emboscar : Action
{
    public override TaskStatus OnUpdate()
    {
        LobsterActionLand lobster = GetComponent<LobsterActionLand>();
        if (lobster != null)
        {
            lobster.Emboscar();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}