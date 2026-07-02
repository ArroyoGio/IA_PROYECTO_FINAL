using UnityEngine;

public class OctopusVehicleLand : SurvivorVehicleLand
{
    private void Awake()
    {
        LoadComponent();
    }

    public void MoveToFoodPrey(Transform prey)
    {
        if (prey == null)
            return;

        ArriveBehaviour(prey.position, 3f);
    }
}
