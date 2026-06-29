using BehaviorDesigner.Runtime.Tasks;

public class BTEmitirPulso : Action
{
    private SphereActionLand sphere;

    public override void OnAwake()
    {
        sphere = GetComponent<SphereActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (sphere == null)
            return TaskStatus.Failure;

        sphere.EmitirPulso();
        return TaskStatus.Success;
    }
}
