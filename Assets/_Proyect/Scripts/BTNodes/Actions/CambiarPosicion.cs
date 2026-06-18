using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CambiarPosicion : Action
{
    public override TaskStatus OnUpdate()
    {
        LobsterActionLand lobster = GetComponent<LobsterActionLand>();
        if (lobster != null)
        {
            lobster.CambiarPosicion();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}