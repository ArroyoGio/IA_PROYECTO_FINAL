using UnityEngine;

public class LobsterVehicleLand : SurvivorVehicleLand
{
    [Header("Lobster Vehicle")]
    public float patrolSpeedMultiplier = 0.45f;
    public float ambushPursuitScale = 0.35f;
    public float preyArriveDistance = 1.8f;
    public float preySlowingDistance = 4f;

    private void Awake()
    {
        LoadComponent();
    }

    public override void Patrullar()
    {
        CacheNormalSpeed();
        maxSpeed = normalMaxSpeed * patrolSpeedMultiplier;
        WanderBehaviour();
    }

    public override void CambiarPosicion()
    {
        Patrullar();
    }

    public void AcercarseAPresaCercana()
    {
        Transform prey = GetNearbyPrey();
        if (prey == null)
            return;

        CacheNormalSpeed();
        maxSpeed = normalMaxSpeed * 0.65f;

        if (Vector3.Distance(transform.position, prey.position) <= preyArriveDistance)
        {
            Stop();
            return;
        }

        AICharacterVehicle preyVehicle = prey.GetComponent<AICharacterVehicle>();
        if (preyVehicle != null)
        {
            Vector3 steering = Pursuit(prey, preyVehicle.GetVelocity()) * ambushPursuitScale;
            ApplySteering(steering);
            return;
        }

        ArriveBehaviour(prey.position, preySlowingDistance);
    }

    private Transform GetNearbyPrey()
    {
        if (blackboard != null)
        {
            object preyObject = blackboard.GetObject("PreyTarget");

            if (preyObject is Transform preyTransform)
                return preyTransform;

            if (preyObject is GameObject preyGameObject)
                return preyGameObject.transform;
        }

        return eye != null && eye.ViewEnemy != null ? eye.ViewEnemy.transform : null;
    }

}
