using UnityEngine;

public abstract class AICharacterAction : AICharacterControl
{
    [Header("Action Settings")]
    public float actionRadius = 10f;
    public LayerMask targetLayer;

    public abstract void PerformAction();
}