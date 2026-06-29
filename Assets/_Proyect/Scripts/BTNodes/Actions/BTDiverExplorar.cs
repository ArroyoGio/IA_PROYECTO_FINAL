using BehaviorDesigner.Runtime.Tasks;

public class BTDiverExplorar : Action
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

        diver.Explorar();
        return TaskStatus.Success;
    }
}
