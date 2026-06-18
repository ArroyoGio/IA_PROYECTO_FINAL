using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TieneMunicion : Conditional
{
    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            int ammo = blackboard.GetInt("Ammo", 0);
            return ammo > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}