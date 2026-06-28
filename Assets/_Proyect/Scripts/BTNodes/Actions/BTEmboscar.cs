using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTEmboscar : Action
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

        Debug.Log("BT EMBOSCAR EJECUTADO");
        lobster.Emboscar();
        return TaskStatus.Success;
    }
}
