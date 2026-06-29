using UnityEngine;

public class SphereVehicleLand : AICharacterVehicle
{
    private const float ReturnDistance = 8f;

    private Vector3 centerPosition;

    private void Awake()
    {
        LoadComponent();
        centerPosition = transform.position;
    }

    private void Update()
    {
        Float();
    }

    public void Float()
    {
        Vector3 position = transform.position;
        position.y = centerPosition.y + Mathf.Sin(Time.time) * 0.3f;
        transform.position = position;
    }

    public void PatrullarSuave()
    {
        WanderBehaviour();
    }

    public void ReturnToCenter()
    {
        if (Vector3.Distance(transform.position, centerPosition) <= ReturnDistance)
            return;

        ArriveBehaviour(centerPosition, 3f);
    }
}
