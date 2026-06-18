using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Salir : Action
{
    public override TaskStatus OnUpdate()
    {
        LobsterActionLand lobster = GetComponent<LobsterActionLand>();
        if (lobster != null)
        {
            lobster.Salir();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}