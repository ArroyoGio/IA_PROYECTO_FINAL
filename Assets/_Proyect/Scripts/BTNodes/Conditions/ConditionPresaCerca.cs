using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConditionPresaCerca : Conditional
{
    private LobsterActionLand lobster;

    public override void OnAwake()
    {
        lobster = GetComponent<LobsterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (lobster == null)
            return TaskStatus.Failure;

        bool preyClose = lobster.IsPreyClose();
        Debug.Log("PRESA CERCA " + (preyClose ? "true" : "false"));

        return preyClose
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
