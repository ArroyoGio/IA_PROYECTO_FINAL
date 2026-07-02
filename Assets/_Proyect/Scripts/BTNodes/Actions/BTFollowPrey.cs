using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTFollowPrey : Action
{
    public float successDistance = 4f;

    private PredatorVehicleLand predatorVehicle;
    private SnakeVehicleLand snakeVehicle;

    public override void OnAwake()
    {
        predatorVehicle = GetComponent<PredatorVehicleLand>();
        snakeVehicle = GetComponent<SnakeVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (predatorVehicle == null)
            return TaskStatus.Failure;

        AIEye eye = predatorVehicle.Eye;
        if (eye == null || eye.ViewEnemy == null)
            return TaskStatus.Failure;

        Transform prey = eye.ViewEnemy.transform;
        if (!IsInLayerHierarchy(prey, "Prey") && !IsInLayerHierarchy(prey, "Human"))
            return TaskStatus.Failure;

        if (snakeVehicle != null)
            snakeVehicle.SeguirPrey();
        else
            predatorVehicle.SeguirPrey();

        float distance = Vector3.Distance(transform.position, prey.position);

        return distance <= successDistance
            ? TaskStatus.Success
            : TaskStatus.Running;
    }

    private bool IsInLayerHierarchy(Transform target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0 || target == null)
            return false;

        Transform current = target;
        while (current != null)
        {
            if (current.gameObject.layer == layer)
                return true;

            current = current.parent;
        }

        Transform[] children = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.layer == layer)
                return true;
        }

        return false;
    }
}
