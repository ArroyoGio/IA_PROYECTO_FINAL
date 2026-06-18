using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ComidaEncontrada : Conditional
{
    public float searchRadius = 10f;

    public override TaskStatus OnUpdate()
    {
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
        foreach (var item in food)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < searchRadius)
            {
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }
}