using UnityEngine;

public class SharkVehicleLand : PredatorVehicleLand
{
    private FuzzyController fuzzyController;
    private float baseSpeed;

    private void Awake()
    {
        LoadComponent();
    }

    private void Update()
    {
        ApplyFuzzySpeed();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        fuzzyController = GetComponent<FuzzyController>();
        baseSpeed = maxSpeed;
    }

    public new void SeguirPrey()
    {
        ApplyFuzzySpeed();
        base.SeguirPrey();
    }

    public new void Patrullar()
    {
        RestoreBaseSpeed();
        base.Patrullar();
    }

    private void ApplyFuzzySpeed()
    {
        if (eye == null || eye.ViewEnemy == null)
        {
            RestoreBaseSpeed();
            return;
        }

        float distance = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        float speedMultiplier = fuzzyController != null
            ? fuzzyController.EvaluateSpeedByDistance(distance)
            : 1f;

        maxSpeed = baseSpeed * speedMultiplier;
    }

    private void RestoreBaseSpeed()
    {
        maxSpeed = baseSpeed;
    }
}
