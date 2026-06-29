using BehaviorDesigner.Runtime.Tasks;

public class ConditionDormido : Conditional
{
    private const float TiempoDespertar = 10f;
    private KrakenActionLand kraken;

    public override void OnAwake()
    {
        kraken = GetComponent<KrakenActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (kraken == null)
            return TaskStatus.Failure;

        return kraken.Fase <= 0f && kraken.TemporizadorDespertar < TiempoDespertar
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
