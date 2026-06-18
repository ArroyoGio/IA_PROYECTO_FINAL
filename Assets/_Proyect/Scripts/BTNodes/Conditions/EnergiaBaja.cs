using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnergiaBaja : Conditional
{
    public float threshold = 30f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float energy = blackboard.GetFloat("Energy", 0f);
            return energy < threshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}