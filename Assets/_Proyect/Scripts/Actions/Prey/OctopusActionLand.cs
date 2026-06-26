using UnityEngine;

public class OctopusActionLand : PreyActionLand
{
    [Header("Octopus Defense")]
    public float camouflage = 100f;
    public float ink = 100f;
    public float inkCost = 40f;
    public float inkRange = 8f;
    public float camouflageDrain = 10f;
    public float camouflageRecover = 8f;

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
        UpdateOctopusBlackboard();
        UpdateRestSpeed();

        if (!isCamouflaging)
            RecuperarCamuflaje();

        if (!isCamouflaging || camouflage <= 0f)
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
        if (camouflage <= 0f)
        {
            RestoreVisibility();
            return;
        }

        isCamouflaging = true;
        camouflage = Mathf.Clamp(camouflage - camouflageDrain * Time.deltaTime, 0f, 100f);

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
        camouflage = Mathf.Clamp(camouflage + camouflageRecover * Time.deltaTime, 0f, 100f);
    }

    public bool PuedeLiberarTinta()
    {
        if (eye == null || eye.ViewEnemy == null)
            return false;

        float distance = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        return distance <= inkRange && ink >= inkCost;
    }

    public void LiberarTinta()
    {
        if (!PuedeLiberarTinta())
            return;

        ink = Mathf.Clamp(ink - inkCost, 0f, 100f);
        fear = Mathf.Clamp(fear + 20f, 0f, maxFear);
        Debug.Log("Octopus liberó tinta");

        PreyVehicleLand preyVehicle = vehicle as PreyVehicleLand;
        if (preyVehicle != null)
            preyVehicle.EvadeEnemy();

        CreateInkCloud();
    }

    private void CreateInkCloud()
    {
        Vector3 back = -transform.forward;
        Vector3 right = transform.right;

        Vector3[] offsets =
        {
            back * 3f,
            back * 4f + right * 1.8f,
            back * 4f - right * 1.8f
        };

        float[] scales = { 5.5f, 4.5f, 4.8f };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject cloud = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cloud.name = "Octopus Ink Cloud";
            cloud.transform.position = transform.position + offsets[i] + Vector3.up * 0.4f;
            cloud.transform.localScale = Vector3.one * scales[i];

            Collider cloudCollider = cloud.GetComponent<Collider>();
            if (cloudCollider != null)
                Destroy(cloudCollider);

            Renderer renderer = cloud.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0f, 0f, 0f, 0.75f);

            Destroy(cloud, 4f);
        }
    }

    private void UpdateOctopusBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("Camouflage", camouflage);
        blackboard.SetFloat("Ink", ink);
        blackboard.SetBool("CamuflajeDisponible", camouflage > 0f);
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
