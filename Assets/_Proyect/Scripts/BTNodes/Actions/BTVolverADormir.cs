using BehaviorDesigner.Runtime.Tasks;

public class BTVolverADormir : Action
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

        kraken.VolverADormir();
        return TaskStatus.Success;
    }
}
