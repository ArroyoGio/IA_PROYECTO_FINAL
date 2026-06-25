using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTBite : Action
{
    private SharkActionLand shark;

    public override void OnAwake()
    {
        shark = GetComponent<SharkActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("BTBite ejecutándose");

        if (shark == null)
        {
            Debug.Log("No encontró SharkActionLand");
            return TaskStatus.Failure;
        }

        shark.Morder();
        return TaskStatus.Success;
    }
}