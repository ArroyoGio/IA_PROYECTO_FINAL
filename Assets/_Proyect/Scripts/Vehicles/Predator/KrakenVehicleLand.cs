using UnityEngine;

public class KrakenVehicleLand : PredatorVehicleLand
{
    private Vector3 startPosition;

    private void Awake()
    {
        LoadComponent();
        startPosition = transform.position;
    }

    public void Idle()
    {
        Stop();
    }

    public void Perseguir()
    {
        SeguirPrey();
    }

    public void Acercarse()
    {
        if (eye == null || eye.ViewEnemy == null)
            return;

        ArriveBehaviour(eye.ViewEnemy.transform.position, 6f);
    }

    public void Volver()
    {
        ArriveBehaviour(startPosition, 8f);
    }
}
