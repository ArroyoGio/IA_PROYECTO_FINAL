using UnityEngine;

public class SharkActionLand : PredatorActionLand
{
    [Header("Shark Attack")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    private AICharacterVehicle vehicle;
    private float normalMaxSpeed;
    private bool hasNormalMaxSpeed;
    private bool isResting;

    private void Awake()
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
        UpdateAI();
        UpdateRestSpeed();
    }

    public void Morder()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        if (eye == null || eye.ViewEnemy == null) return;

        Transform prey = eye.ViewEnemy.transform;
        if (!IsTargetInRange(prey)) return;

        Attack(prey);

        lastAttackTime = Time.time;

        hunger = Mathf.Max(0, hunger - 30f);
        energy = Mathf.Max(0, energy - 5f);
        aggressiveness = Mathf.Min(100, aggressiveness + 5f);
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
