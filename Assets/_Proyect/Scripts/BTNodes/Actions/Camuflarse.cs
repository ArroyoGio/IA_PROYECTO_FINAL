using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Camuflarse : Action
{
    public override TaskStatus OnUpdate()
    {
        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.ActivarCamuflaje();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}