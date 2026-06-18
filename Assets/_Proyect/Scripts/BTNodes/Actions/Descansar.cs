using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Descansar : Action
{
    public override TaskStatus OnUpdate()
    {
        FishActionLand fish = GetComponent<FishActionLand>();
        if (fish != null)
        {
            fish.Descansar();
            return TaskStatus.Running;
        }

        SharkActionLand shark = GetComponent<SharkActionLand>();
        if (shark != null)
        {
            shark.Descansar();
            return TaskStatus.Running;
        }

        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
        {
            diver.Rest();
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}