using BehaviorDesigner.Runtime.Tasks;

public class BTBuscarZonaSegura : Action
{
    private DiverActionLand diver;

    public override void OnAwake()
    {
        diver = GetComponent<DiverActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (diver == null)
            return TaskStatus.Failure;

        diver.BuscarZonaSegura();
        return TaskStatus.Success;
    }
}
