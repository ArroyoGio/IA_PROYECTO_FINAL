using UnityEngine;

public class DolphinActionLand : PreyActionLand
{
    [Header("Dolphin Mind")]
    public float curiosity = 60f;
    public float confidence = 50f;

    [Header("Dolphin Thresholds")]
    public float curiosityHighThreshold = 65f;
    public float confidenceSafeThreshold = 35f;

    private DolphinVehicleLand dolphinVehicle;
    private AICharacterVehicle vehicle;
    private float normalMaxSpeed;
    private bool hasNormalMaxSpeed;
    private bool isSwimmingSlow;
    private float distanciaAmenaza;
    private Transform currentCuriosityTarget;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        dolphinVehicle = GetComponent<DolphinVehicleLand>();
        vehicle = GetComponent<AICharacterVehicle>();
    }

    private void Update()
    {
        UpdateAI();
        UpdateSlowSwimSpeed();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateDolphinState();
    }

    public void Explorar()
    {
        if (dolphinVehicle != null)
            dolphinVehicle.Patrullar();

        curiosity = Mathf.Clamp(curiosity + 8f * Time.deltaTime, 0f, 100f);
        confidence = Mathf.Clamp(confidence + 4f * Time.deltaTime, 0f, 100f);
        energy = Mathf.Clamp(energy - 0.5f * Time.deltaTime, 0f, maxEnergy);
    }

    public void EvadirAmenaza()
    {
        if (dolphinVehicle != null)
            dolphinVehicle.EvadeEnemy();

        fear = Mathf.Clamp(fear + 10f * Time.deltaTime, 0f, maxFear);
        confidence = Mathf.Clamp(confidence - 8f * Time.deltaTime, 0f, 100f);
        energy = Mathf.Clamp(energy - 4f * Time.deltaTime, 0f, maxEnergy);
    }

    public void AcercarseCurioso()
    {
        if (curiosity < curiosityHighThreshold || confidence < confidenceSafeThreshold || !HasCuriosityTarget())
            return;

        if (dolphinVehicle != null)
            dolphinVehicle.AcercarseCurioso();

        curiosity = Mathf.Clamp(curiosity - 3f * Time.deltaTime, 0f, 100f);
        confidence = Mathf.Clamp(confidence + 2f * Time.deltaTime, 0f, 100f);
        energy = Mathf.Clamp(energy - 1.5f * Time.deltaTime, 0f, maxEnergy);
    }

    public bool HasCuriosityTarget()
    {
        if (eye != null && eye.ViewEnemy != null)
        {
            currentCuriosityTarget = null;
            UpdateCuriosityTargetBlackboard();
            return false;
        }

        currentCuriosityTarget = FindClosestCuriosityTarget();
        UpdateCuriosityTargetBlackboard();
        return currentCuriosityTarget != null;
    }

    public bool IsCuriosityTargetReached()
    {
        if (currentCuriosityTarget == null)
            return false;

        float arriveDistance = dolphinVehicle != null ? dolphinVehicle.curiousArriveDistance : 3f;
        return Vector3.Distance(transform.position, currentCuriosityTarget.position) <= arriveDistance;
    }

    public void NadarLento()
    {
        isSwimmingSlow = true;
        ApplySlowSpeed();

        if (dolphinVehicle != null)
            dolphinVehicle.Patrullar();

        energy = Mathf.Clamp(energy + 12f * Time.deltaTime, 0f, maxEnergy);
        fear = Mathf.Clamp(fear - 4f * Time.deltaTime, 0f, maxFear);
        confidence = Mathf.Clamp(confidence + 3f * Time.deltaTime, 0f, 100f);

        if (energy > UpEnergiaBaja + 10f)
        {
            RestoreNormalSpeed();
            isSwimmingSlow = false;
        }
    }

    private void UpdateDolphinState()
    {
        Transform threat = eye != null && eye.ViewEnemy != null ? eye.ViewEnemy.transform : null;
        distanciaAmenaza = threat != null ? Vector3.Distance(transform.position, threat.position) : float.MaxValue;

        if (threat != null)
        {
            curiosity = Mathf.Clamp(curiosity - 3f * Time.deltaTime, 0f, 100f);
            confidence = Mathf.Clamp(confidence - 2f * Time.deltaTime, 0f, 100f);
        }
        else
        {
            curiosity = Mathf.Clamp(curiosity + 2f * Time.deltaTime, 0f, 100f);
            confidence = Mathf.Clamp(confidence + 1f * Time.deltaTime, 0f, 100f);
        }

        if (blackboard != null)
        {
            blackboard.SetFloat("Curiosity", curiosity);
            blackboard.SetFloat("Confidence", confidence);
            blackboard.SetFloat("DistanciaAmenaza", distanciaAmenaza);
            blackboard.SetBool("CuriosidadAlta", curiosity >= curiosityHighThreshold);
            blackboard.SetBool("ConfianzaAlta", confidence >= confidenceSafeThreshold);
        }
    }

    private Transform FindClosestCuriosityTarget()
    {
        int curiosityMask = LayerMask.GetMask("Human", "Ambient");
        if (curiosityMask == 0)
            return null;

        Collider[] targets = Physics.OverlapSphere(transform.position, actionRadius, curiosityMask);
        Transform closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    private void UpdateCuriosityTargetBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetObject("CuriosityTarget", currentCuriosityTarget);
    }

    private void ApplySlowSpeed()
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

        vehicle.maxSpeed = normalMaxSpeed * 0.45f;
    }

    private void UpdateSlowSwimSpeed()
    {
        if (!isSwimmingSlow || energy > UpEnergiaBaja + 10f)
            RestoreNormalSpeed();

        isSwimmingSlow = false;
    }

    private void RestoreNormalSpeed()
    {
        if (vehicle == null || !hasNormalMaxSpeed)
            return;

        vehicle.maxSpeed = normalMaxSpeed;
    }
}
