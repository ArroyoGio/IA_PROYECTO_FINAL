using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RecargarCamuflaje : Action
{
    public override TaskStatus OnUpdate()
    {
        OctopusActionLand octopus = GetComponent<OctopusActionLand>();
        if (octopus != null)
        {
            octopus.RecargarCamuflaje();
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}