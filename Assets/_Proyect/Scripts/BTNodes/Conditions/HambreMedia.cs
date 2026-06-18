using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HambreMedia : Conditional
{
    public float minThreshold = 30f;
    public float maxThreshold = 70f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float hunger = blackboard.GetFloat("Hunger", 0f);
            return hunger > minThreshold && hunger <= maxThreshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}