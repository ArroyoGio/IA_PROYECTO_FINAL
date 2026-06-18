using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Comer : Action
{
    public override TaskStatus OnUpdate()
    {
        FishActionLand fish = GetComponent<FishActionLand>();
        if (fish != null)
        {
            fish.Comer();
            return TaskStatus.Success;
        }

        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.Comer();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}