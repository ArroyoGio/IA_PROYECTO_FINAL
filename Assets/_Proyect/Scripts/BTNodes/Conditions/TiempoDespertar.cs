using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TiempoDespertar : Conditional
{
    public float wakeTime = 30f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float wakeTimer = blackboard.GetFloat("WakeTimer", 0f);
            return wakeTimer >= wakeTime ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}