using UnityEngine;

public class DiverActionLand : HumanActionLand
{
    [Header("Diver Settings")]
    public float oxygen = 100f;
    public float danger = 0f;
    public float explorationInterest = 70f;
    public float scanRadius = 15f;
    public float scanCooldown = 4f;

    private const float OxygenDrain = 2f;
    private const float SafeHeight = 8f;
    private const float AscendSpeed = 3f;
    private const float OxygenRecoverRate = 35f;
    private const float DangerDecayRate = 40f;
    private const float ScanDistance = 3f;

    private float lastScanTime = -999f;
    private Transform currentScanTarget;
    private bool isRecovering;

    private void Awake()
    {
        LoadComponent();
    }

    private void Update()
    {
        UpdateAI();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateDiverState();
        UpdateDiverBlackboard();
    }

    public void Explorar()
    {
        if (diverVehicle != null)
            diverVehicle.Patrullar();

        explorationInterest = Mathf.Clamp(explorationInterest - 2f * Time.deltaTime, 0f, 100f);
    }

    public void BuscarZonaSegura()
    {
        isRecovering = true;

        Transform predator = GetDetectedPredator();
        if (predator != null && diverVehicle != null)
            diverVehicle.FleeFrom(predator);

        UpdateRecovery();
    }

    public bool Escanear()
    {
        currentScanTarget = GetClosestScanTarget();
        if (currentScanTarget == null)
            return false;

        float distance = Vector3.Distance(transform.position, currentScanTarget.position);
        if (distance > ScanDistance)
        {
            if (diverVehicle != null)
                diverVehicle.MoveToTarget(currentScanTarget.position);

            return true;
        }

        if (diverVehicle != null)
            diverVehicle.Stop();

        lastScanTime = Time.time;
        explorationInterest = Mathf.Clamp(explorationInterest + 15f, 0f, 100f);

        if (blackboard != null)
            blackboard.SetFloat("ScannedCreatures", 1f);

        return true;
    }

    private void UpdateDiverState()
    {
        if (isRecovering)
        {
            UpdateRecovery();
            return;
        }

        oxygen = Mathf.Clamp(oxygen - OxygenDrain * Time.deltaTime, 0f, 100f);

        bool seesDanger = GetDetectedPredator() != null;
        danger = seesDanger
            ? Mathf.Clamp(danger + 10f * Time.deltaTime, 0f, 100f)
            : Mathf.Clamp(danger - 5f * Time.deltaTime, 0f, 100f);
    }

    private void UpdateRecovery()
    {
        if (transform.position.y < SafeHeight)
            transform.position += Vector3.up * AscendSpeed * Time.deltaTime;

        oxygen = Mathf.Clamp(oxygen + OxygenRecoverRate * Time.deltaTime, 0f, 100f);
        danger = Mathf.Clamp(danger - DangerDecayRate * Time.deltaTime, 0f, 100f);

        if (oxygen >= 90f && danger <= 20f)
        {
            isRecovering = false;
            explorationInterest = 100f;
        }
    }

    public bool CanScan()
    {
        return Time.time >= lastScanTime + scanCooldown &&
               !isRecovering &&
               danger <= 60f &&
               GetDetectedPredator() == null &&
               GetClosestScanTarget() != null;
    }

    public bool HasHighDanger()
    {
        return danger > 60f || GetDetectedPredator() != null;
    }

    private Transform GetDetectedPredator()
    {
        if (eye == null || eye.ViewEnemy == null)
            return null;

        Transform target = eye.ViewEnemy.transform;
        return IsInLayerHierarchy(target, "Predator") ? target : null;
    }

    private Transform GetClosestScanTarget()
    {
        int scanMask = LayerMask.GetMask("Prey", "Survivor", "Ambient");
        Collider[] targets = Physics.OverlapSphere(transform.position, scanRadius, scanMask);

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

    private bool IsInLayerHierarchy(Transform target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0 || target == null)
            return false;

        Transform current = target;
        while (current != null)
        {
            if (current.gameObject.layer == layer)
                return true;

            current = current.parent;
        }

        return false;
    }

    private void UpdateDiverBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("Oxygen", oxygen);
        blackboard.SetFloat("Energy", energy);
        blackboard.SetFloat("Danger", danger);
        blackboard.SetFloat("ExplorationInterest", explorationInterest);
        blackboard.SetFloat("ScanRadius", scanRadius);
        blackboard.SetFloat("ScannedCreatures", currentScanTarget != null ? 1f : 0f);
        blackboard.SetBool("OxigenoBajo", oxygen < 70f);
        blackboard.SetBool("EnergiaBaja", energy < UpEnergiaBaja);
        blackboard.SetBool("PeligroAlto", HasHighDanger());
        blackboard.SetBool("PuedeEscanear", CanScan());
        blackboard.SetBool("InteresExploracionAlto", explorationInterest > 40f);
    }
}
