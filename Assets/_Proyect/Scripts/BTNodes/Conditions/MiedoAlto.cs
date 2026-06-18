using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MiedoAlto : Conditional
{
    public float threshold = 70f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float fear = blackboard.GetFloat("Fear", 0f);
            return fear > threshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}