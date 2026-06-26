using BehaviorDesigner.Runtime.Tasks;

public class ConditionVeDepredador : Conditional
{
    private AIEye eye;

    public override void OnAwake()
    {
        eye = GetComponent<AIEye>();
    }

    public override TaskStatus OnUpdate()
    {
        return eye != null && eye.ViewEnemy != null
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
