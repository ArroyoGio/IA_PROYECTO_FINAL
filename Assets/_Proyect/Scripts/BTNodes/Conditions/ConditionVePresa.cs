using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConditionVePresa : Conditional
{
    private AIEye eye;

    public override void OnAwake()
    {
        eye = GetComponent<AIEye>();
    }

    public override TaskStatus OnUpdate()
    {
        if (eye == null || eye.ViewEnemy == null)
            return TaskStatus.Failure;

        Transform target = eye.ViewEnemy.transform;
        return IsInLayerHierarchy(target, "Prey") || IsInLayerHierarchy(target, "Human")
            ? TaskStatus.Success
            : TaskStatus.Failure;
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
