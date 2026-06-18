using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BuscarComida : Action
{
    public override TaskStatus OnUpdate()
    {
        FishActionLand fish = GetComponent<FishActionLand>();
        if (fish != null)
        {
            fish.BuscarComida();
            return TaskStatus.Running;
        }

        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.BuscarComida();
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}