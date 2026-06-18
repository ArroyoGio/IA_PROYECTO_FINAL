using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnergiaBioluminiscenteBaja : Conditional
{
    public float threshold = 20f;

    public override TaskStatus OnUpdate()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        if (blackboard != null)
        {
            float bioEnergy = blackboard.GetFloat("BioEnergy", 0f);
            return bioEnergy < threshold ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}