using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EvadeNode : Action
{
    public override TaskStatus OnUpdate()
    {
        PreyActionLand prey = GetComponent<PreyActionLand>();
        if (prey != null)
        {
            prey.Evade();
            return TaskStatus.Running;
        }

        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.Evade();
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}