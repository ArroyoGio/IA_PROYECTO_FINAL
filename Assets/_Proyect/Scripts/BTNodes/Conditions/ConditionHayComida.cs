using BehaviorDesigner.Runtime.Tasks;

public class ConditionHayComida : Conditional
{
    private AIFoodSensor foodSensor;

    public override void OnAwake()
    {
        foodSensor = GetComponent<AIFoodSensor>();
    }

    public override TaskStatus OnUpdate()
    {
        if (foodSensor == null)
            return TaskStatus.Failure;

        foodSensor.ScanFood();
        return foodSensor.HasFood ? TaskStatus.Success : TaskStatus.Failure;
    }
}
