using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Patrullar : Action
{
    public override TaskStatus OnUpdate()
    {
        SharkActionLand shark = GetComponent<SharkActionLand>();
        if (shark != null)
        {
            shark.Patrullar();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}