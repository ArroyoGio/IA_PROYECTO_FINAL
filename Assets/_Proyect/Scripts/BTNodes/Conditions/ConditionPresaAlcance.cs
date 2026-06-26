using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConditionPresaAlcance : Conditional
{
    private PredatorActionLand predator;

    public override void OnAwake()
    {
        predator = GetComponent<PredatorActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (predator == null || predator.Eye == null || predator.Eye.ViewEnemy == null)
            return TaskStatus.Failure;

        return predator.IsTargetInRange(predator.Eye.ViewEnemy.transform)
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}