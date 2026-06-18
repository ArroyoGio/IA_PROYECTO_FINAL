using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SeleccionarFase : Action
{
    public SharedInt currentPhase;

    public override TaskStatus OnUpdate()
    {
        KrakenActionLand kraken = GetComponent<KrakenActionLand>();
        if (kraken != null)
        {
            currentPhase.Value = kraken.SeleccionarFase();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}