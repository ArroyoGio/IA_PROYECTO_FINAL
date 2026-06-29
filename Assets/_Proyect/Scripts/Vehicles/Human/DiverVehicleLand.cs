using UnityEngine;

public class DiverVehicleLand : AICharacterVehicle
{
    private void Awake()
    {
        LoadComponent();
    }

    public void Patrullar()
    {
        WanderBehaviour();
    }

    public void MoveToTarget(Vector3 target)
    {
        ArriveBehaviour(target, 3f);
    }

    public void FleeFrom(Transform predator)
    {
        if (predator == null)
            return;

        FleeBehaviour(predator.position);
    }

}
