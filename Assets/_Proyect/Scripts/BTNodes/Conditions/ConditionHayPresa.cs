using BehaviorDesigner.Runtime.Tasks;

public class ConditionHayPresa : Conditional
{
    private KrakenActionLand kraken;

    public override void OnAwake()
    {
        kraken = GetComponent<KrakenActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (kraken == null || kraken.Eye == null)
            return TaskStatus.Failure;

        return kraken.Eye.ViewEnemy != null
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
