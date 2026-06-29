using BehaviorDesigner.Runtime.Tasks;

public class BTEscanear : Action
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

        return diver.Escanear()
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
