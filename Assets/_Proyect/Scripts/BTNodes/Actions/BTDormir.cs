using BehaviorDesigner.Runtime.Tasks;

public class BTDormir : Action
{
    private KrakenActionLand kraken;

    public override void OnAwake()
    {
        kraken = GetComponent<KrakenActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (kraken == null)
            return TaskStatus.Failure;

        kraken.Dormir();
        return TaskStatus.Success;
    }
}
