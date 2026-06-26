using BehaviorDesigner.Runtime.Tasks;

public class BTDescansar : Action
{
    private FishActionLand fish;
    private OctopusActionLand octopus;
    private SharkActionLand shark;

    public override void OnAwake()
    {
        fish = GetComponent<FishActionLand>();
        octopus = GetComponent<OctopusActionLand>();
        shark = GetComponent<SharkActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (fish != null)
        {
            fish.Descansar();
            return TaskStatus.Success;
        }

        if (octopus != null)
        {
            octopus.Descansar();
            return TaskStatus.Success;
        }

        if (shark != null)
        {
            shark.Descansar();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
