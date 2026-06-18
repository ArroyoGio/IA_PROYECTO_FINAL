using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class LiberarTinta : Action
{
    public override TaskStatus OnUpdate()
    {
        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.LiberarTinta();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}