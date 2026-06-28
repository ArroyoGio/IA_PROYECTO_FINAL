using UnityEngine;

public class OctopusActionLand : PreyActionLand
{
    [Header("Octopus Ink")]
    public float ink = 100f;
    public float inkCost = 40f;
    public float inkRange = 8f;
    public float inkCooldown = 3f;
    public float inkCloudDuration = 6f;
    public float inkRecoverRate = 15f;
    public ParticleSystem inkParticlePrefab;

    private const float MaxCamouflage = 100f;
    private const float CamouflageDrain = 10f;
    private const float CamouflageRecover = 8f;

    private float camouflageValue = MaxCamouflage;
    private float lastInkTime = -999f;

    public float camouflage => camouflageValue;

    private AICharacterVehicle vehicle;
    private float normalMaxSpeed;
    private bool hasNormalMaxSpeed;
    private bool isResting;

    private float normalEyeDistance;
    private bool hasNormalEyeDistance;
    private bool isCamouflaging;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        vehicle = GetComponent<AICharacterVehicle>();

        if (eye != null && !hasNormalEyeDistance)
        {
            normalEyeDistance = eye.distance;
            hasNormalEyeDistance = true;
        }
    }

    private void Update()
    {
        base.UpdateAI();
        RecoverInk();
        UpdateOctopusBlackboard();
        UpdateRestSpeed();

        if (!isCamouflaging)
            RecuperarCamuflaje();

        if (!isCamouflaging || camouflageValue <= 0f)
            RestoreVisibility();

        isCamouflaging = false;
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

    public void Camuflarse()
    {
        if (camouflageValue <= 0f)
        {
            RestoreVisibility();
            return;
        }

        isCamouflaging = true;
        camouflageValue = Mathf.Clamp(camouflageValue - CamouflageDrain * Time.deltaTime, 0f, MaxCamouflage);

        if (eye != null)
        {
            if (!hasNormalEyeDistance)
            {
                normalEyeDistance = eye.distance;
                hasNormalEyeDistance = true;
            }

            eye.distance = normalEyeDistance * 0.4f;
        }
    }

    public void RecuperarCamuflaje()
    {
        camouflageValue = Mathf.Clamp(camouflageValue + CamouflageRecover * Time.deltaTime, 0f, MaxCamouflage);
    }

    private void RecoverInk()
    {
        if (ink < 100f)
        {
            ink += inkRecoverRate * Time.deltaTime;
            ink = Mathf.Clamp(ink, 0f, 100f);
        }
    }

    public bool PuedeLiberarTinta()
    {
        if (eye == null || eye.ViewEnemy == null)
            return false;

        float distance = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        bool isInRange = distance <= inkRange;
        bool hasInk = ink >= inkCost;
        bool cooldownReady = Time.time >= lastInkTime + inkCooldown;

        return isInRange && hasInk && cooldownReady;
    }

    public void LiberarTinta()
    {
        if (eye == null || eye.ViewEnemy == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        if (distance > inkRange)
        {
            return;
        }

        if (ink < inkCost)
        {
            return;
        }

        if (Time.time < lastInkTime + inkCooldown)
        {
            return;
        }

        ink = Mathf.Clamp(ink - inkCost, 0f, 100f);
        lastInkTime = Time.time;
        fear = Mathf.Clamp(fear + 20f, 0f, maxFear);

        if (inkParticlePrefab != null)
        {
            ParticleSystem inkEffect = Instantiate(inkParticlePrefab, transform.position, Quaternion.identity);
            inkEffect.Play();
            Destroy(inkEffect.gameObject, inkCloudDuration);
        }

        PreyVehicleLand preyVehicle = vehicle as PreyVehicleLand;
        if (preyVehicle != null)
            preyVehicle.EvadeEnemy();
    }

    private void UpdateOctopusBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("Camouflage", camouflageValue);
        blackboard.SetFloat("Ink", ink);
        blackboard.SetBool("CamuflajeDisponible", camouflageValue > 0f);
        blackboard.SetBool("PuedeLiberarTinta", PuedeLiberarTinta());
    }

    private void RestoreVisibility()
    {
        if (eye != null && hasNormalEyeDistance)
            eye.distance = normalEyeDistance;
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
