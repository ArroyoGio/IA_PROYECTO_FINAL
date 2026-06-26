using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConditionVeDepredador : Conditional
{
    private AIEye eye;

    public override void OnAwake()
    {
        eye = GetComponent<AIEye>();
    }

    public override TaskStatus OnUpdate()
    {
        bool ve = eye != null && eye.ViewEnemy != null;
        Debug.Log(gameObject.name + " ConditionVeDepredador = " + ve);

        return ve ? TaskStatus.Success : TaskStatus.Failure;
    }
}