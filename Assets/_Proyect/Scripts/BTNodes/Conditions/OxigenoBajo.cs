using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class OxigenoBajo : Conditional
{
    public float threshold = 30f;

    public override TaskStatus OnUpdate()
    {
        DiverActionLand diver = GetComponent<DiverActionLand>();
        if (diver != null)
            return diver.oxygen < 70f ? TaskStatus.Success : TaskStatus.Failure;

        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float oxygen = blackboard.GetFloat("Oxygen", 0f);
            return oxygen < threshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}
