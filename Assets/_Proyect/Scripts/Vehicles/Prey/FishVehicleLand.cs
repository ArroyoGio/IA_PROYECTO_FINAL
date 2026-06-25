using UnityEngine;

public class FishVehicleLand : PreyVehicleLand
{
    private void Awake()
    {
        LoadComponent();
    }

    private void Update()
    {
        if (SeeEnemy())
            EvadeEnemy();
        else
            Patrullar();
    }
}