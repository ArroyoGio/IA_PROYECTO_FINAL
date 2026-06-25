using UnityEngine;

public class SharkVehicleLand : PredatorVehicleLand
{
    private void Awake()
    {
        LoadComponent();
    }
    private void Update()
    {
        Debug.Log(eye.ViewEnemy);

        if (eye != null && eye.ViewEnemy != null)
        {
            SeguirPrey();
        }
        else
        {
            Patrullar();
        }
    }
}