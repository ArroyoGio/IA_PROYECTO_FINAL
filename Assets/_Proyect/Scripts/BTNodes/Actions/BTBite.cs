using BehaviorDesigner.Runtime.Tasks;

public class BTBite : Action
{
    private SharkActionLand shark;

    public override void OnAwake()
    {
        shark = GetComponent<SharkActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (shark == null)
            return TaskStatus.Failure;

        shark.Morder();
        return TaskStatus.Success;
    }
}
