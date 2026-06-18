using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BuscarCardumen : Action
{
    public override TaskStatus OnUpdate()
    {
        FishActionLand fish = GetComponent<FishActionLand>();
        if (fish != null)
        {
            fish.MantenerCardumen();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}