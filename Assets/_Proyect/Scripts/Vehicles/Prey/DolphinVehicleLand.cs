using UnityEngine;

public class DolphinVehicleLand : PreyVehicleLand
{
    [Header("Dolphin Vehicle")]
    public float curiousArriveDistance = 3f;
    public float curiousSlowingDistance = 8f;
    public float curiousPursuitScale = 0.35f;

    private void Awake()
    {
        LoadComponent();
    }

    public void AcercarseCurioso()
    {
        Transform target = GetCuriosityTarget();
        if (target == null)
            return;

        if (Vector3.Distance(transform.position, target.position) <= curiousArriveDistance)
        {
            Stop();
            return;
        }

        AICharacterVehicle targetVehicle = target.GetComponent<AICharacterVehicle>();
        if (targetVehicle != null)
        {
            Vector3 steering = Pursuit(target, targetVehicle.GetVelocity()) * curiousPursuitScale;
            ApplySteering(steering);
            return;
        }

        ArriveBehaviour(target.position, curiousSlowingDistance);
    }

    private Transform GetCuriosityTarget()
    {
        if (blackboard != null)
        {
            object targetObject = blackboard.GetObject("CuriosityTarget");

            if (targetObject is Transform targetTransform)
                return targetTransform;

            if (targetObject is GameObject targetGameObject)
                return targetGameObject.transform;
        }

        return eye != null && eye.ViewEnemy != null ? eye.ViewEnemy.transform : null;
    }
}
