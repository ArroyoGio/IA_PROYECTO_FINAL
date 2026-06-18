using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PacienciaBaja : Conditional
{
    public float threshold = 30f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float patience = blackboard.GetFloat("Patience", 0f);
            return patience < threshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}