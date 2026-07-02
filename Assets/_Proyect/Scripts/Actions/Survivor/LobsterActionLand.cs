using UnityEngine;

public class LobsterActionLand : SurvivorActionLand
{
    [Header("Lobster Mind")]
    public float patience = 70f;
    public float buryDepth = 0.4f;
    public float ambushRange = 4f;
    public float ambushDamage = 15f;

    private const float AttackCooldown = 1.5f;
    private const float PatienceRecoverRate = 8f;
    private const float PatienceCost = 20f;
    private const float HideSpeedMultiplier = 0.02f;

    private LobsterVehicleLand lobsterVehicle;
    private float lastAttackTime = -999f;
    private bool isBuried;
    private Vector3 originalPosition;
    private bool hasOriginalPosition;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        lobsterVehicle = survivorVehicle as LobsterVehicleLand;
    }

    private void Update()
    {
        UpdateAI();
    }

    public void Enterrarse()
    {
        hiding = 100f;
        LowerForBurrow();

        if (lobsterVehicle != null)
            lobsterVehicle.Stop();

        if (survivorVehicle != null)
            survivorVehicle.SetSpeedMultiplier(HideSpeedMultiplier);

        patience = Mathf.Clamp(patience + PatienceRecoverRate * Time.deltaTime, 0f, 100f);
    }

    public void Emboscar()
    {
        hiding = 0f;
        RestoreFromBurrow();
        RestoreLobsterNormalSpeed();

        if (!IsPreyClose())
            return;

        if (currentPrey == null)
            return;

        if (Time.time < lastAttackTime + AttackCooldown)
        {
            return;
        }

        Vector3 toPrey = currentPrey.position - transform.position;
        float distanceToPrey = toPrey.magnitude;
        if (distanceToPrey > 0.01f)
        {
            float lungeDistance = Mathf.Min(distanceToPrey, ambushRange * 0.75f);
            transform.position += toPrey.normalized * lungeDistance;
        }

        if (lobsterVehicle != null)
            lobsterVehicle.Stop();

        if (Vector3.Distance(transform.position, currentPrey.position) > ambushRange)
        {
            return;
        }

        HealthBase preyHealth = currentPrey.GetComponentInParent<HealthBase>();
        if (preyHealth == null)
        {
            return;
        }

        if (preyHealth.IsDead)
            return;

        preyHealth.ApplyDamage(ambushDamage, WeaponType.Normal);
        lastAttackTime = Time.time;
        hunger = Mathf.Clamp(hunger - 20f, 0f, maxHunger);
        patience = Mathf.Clamp(patience - PatienceCost, 0f, 100f);
    }

    public void CambiarPosicion()
    {
        hiding = 0f;
        RestoreFromBurrow();
        RestoreLobsterNormalSpeed();

        if (lobsterVehicle != null)
            lobsterVehicle.CambiarPosicion();

        patience = Mathf.Clamp(patience + 10f * Time.deltaTime, 0f, 100f);
    }

    public void Defenderse()
    {
        if (!HasPredatorThreat())
            return;

        threat = ThreatHighValue;
        hiding = 100f;
    }

    public override bool IsPreyClose()
    {
        if (eye == null || eye.ViewEnemy == null)
        {
            currentPrey = null;
            return false;
        }

        Transform target = eye.ViewEnemy.transform;
        if (!IsInLayerHierarchy(target, "Prey"))
        {
            currentPrey = null;
            return false;
        }

        currentPrey = target;
        return Vector3.Distance(transform.position, currentPrey.position) <= ambushRange;
    }

    protected override void UpdateSurvivorState()
    {
        base.UpdateSurvivorState();

        if (threat <= 60f)
        {
            RestoreFromBurrow();
            RestoreLobsterNormalSpeed();
        }

        if (!isBuried)
        {
            patience = Mathf.Clamp(patience + PatienceRecoverRate * Time.deltaTime, 0f, 100f);
        }
    }

    protected override void UpdateSurvivorBlackboard()
    {
        base.UpdateSurvivorBlackboard();

        if (blackboard != null)
            blackboard.SetFloat("Patience", patience);
    }

    private bool LowerForBurrow()
    {
        if (isBuried)
            return false;

        originalPosition = transform.position;
        hasOriginalPosition = true;

        transform.position = originalPosition + Vector3.down * buryDepth;
        isBuried = true;
        return true;
    }

    private void RestoreFromBurrow()
    {
        if (!isBuried)
            return;

        if (hasOriginalPosition)
            transform.position = originalPosition;
        else
            transform.position += Vector3.up * buryDepth;

        isBuried = false;
        hiding = 0f;
    }

    private void RestoreLobsterNormalSpeed()
    {
        if (survivorVehicle != null)
            survivorVehicle.RestoreNormalSpeed();
    }

}
