using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EsferaCercana : Conditional
{
    public float range = 10f;

    public override TaskStatus OnUpdate()
    {
        GameObject sphere = GameObject.FindGameObjectWithTag("Sphere");
        if (sphere != null)
        {
            float distance = Vector3.Distance(transform.position, sphere.transform.position);
            return distance < range ? TaskStatus.Success : TaskStatus.Failure;
        }
        return TaskStatus.Failure;
    }
}