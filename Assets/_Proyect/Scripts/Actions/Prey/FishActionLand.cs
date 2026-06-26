using UnityEngine;

public class FishActionLand : PreyActionLand
{
    private AICharacterVehicle vehicle;
    private float normalMaxSpeed;
    private bool hasNormalMaxSpeed;
    private bool isResting;

    void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        vehicle = GetComponent<AICharacterVehicle>();
    }

    private void Update()
    {
        base.UpdateAI();
        UpdateRestSpeed();
    }

    public void Comer()
    {
        hunger = Mathf.Max(0, hunger - 30f);
        energy = Mathf.Min(maxEnergy, energy + 10f);
    }

    public void Descansar()
    {
        isResting = true;
        ApplyRestSpeed();

        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (energy > UpEnergiaBaja + 10f)
        {
            RestoreNormalSpeed();
            isResting = false;
        }
    }

    private void ApplyRestSpeed()
    {
        if (vehicle == null)
            vehicle = GetComponent<AICharacterVehicle>();

        if (vehicle == null)
            return;

        if (!hasNormalMaxSpeed)
        {
            normalMaxSpeed = vehicle.maxSpeed;
            hasNormalMaxSpeed = true;
        }

        vehicle.maxSpeed = normalMaxSpeed * 0.4f;
    }

    private void UpdateRestSpeed()
    {
        if (!isResting || energy > UpEnergiaBaja + 10f)
            RestoreNormalSpeed();

        isResting = false;
    }

    private void RestoreNormalSpeed()
    {
        if (vehicle == null || !hasNormalMaxSpeed)
            return;

        vehicle.maxSpeed = normalMaxSpeed;
    }
}
