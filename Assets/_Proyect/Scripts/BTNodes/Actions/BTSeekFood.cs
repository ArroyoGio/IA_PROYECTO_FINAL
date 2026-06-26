using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTSeekFood : Action
{
    public float arriveDistance = 1.5f;
    public float slowingDistance = 5f;

    private AIFoodSensor foodSensor;
    private AICharacterVehicle vehicle;

    public override void OnAwake()
    {
        foodSensor = GetComponent<AIFoodSensor>();
        vehicle = GetComponent<AICharacterVehicle>();
    }

    public override TaskStatus OnUpdate()
    {
        if (foodSensor == null || vehicle == null)
            return TaskStatus.Failure;

        foodSensor.ScanFood();

        Transform foodTarget = foodSensor.FoodTarget;
        if (foodTarget == null)
            return TaskStatus.Failure;

        float distance = Vector3.Distance(transform.position, foodTarget.position);
        if (distance <= arriveDistance)
            return TaskStatus.Success;

        vehicle.ArriveBehaviour(foodTarget.position, slowingDistance);
        return TaskStatus.Running;
    }
}
