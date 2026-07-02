using UnityEngine;

public class OctopusActionLand : SurvivorActionLand
{
    [Header("Octopus Camouflage")]
    public float camouflage = 100f;
    public float maxCamouflage = 100f;
    public float fear = 0f;
    public float maxFear = 100f;
    public float DownMiedoAlto = 100f;

    [Header("Octopus Ink")]
    public float ink = 100f;
    public float inkCost = 40f;
    public float inkRange = 8f;
    public float inkCooldown = 3f;
    public float inkCloudDuration = 6f;
    public float inkRecoverRate = 15f;
    public ParticleSystem inkParticlePrefab;

    [Header("Octopus Prey Food")]
    public float preyFoodRange = 10f;
    public float preyEatRange = 2f;
    public float preyEatDamage = 20f;
    public float preyEatCooldown = 1.5f;

    private const float CamouflageDrain = 10f;
    private const float CamouflageRecover = 8f;
    private const float CamouflageAlpha = 0.35f;

    private float lastInkTime = -999f;
    private float lastPreyEatTime = -999f;

    private bool isResting;
    private Transform currentFoodPrey;

    private float normalEyeDistance;
    private bool hasNormalEyeDistance;
    private bool isCamouflaging;
    private Renderer[] octopusRenderers;
    private Material[][] octopusMaterials;
    private Color[][] originalMaterialColors;
    private int[][] originalRenderQueues;
    private float[][] originalSurfaceValues;
    private float[][] originalBlendValues;
    private float[][] originalSrcBlendValues;
    private float[][] originalDstBlendValues;
    private float[][] originalZWriteValues;
    private bool[][] originalTransparentKeywords;
    private bool isVisualCamouflaged;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();

        if (eye != null && !hasNormalEyeDistance)
        {
            normalEyeDistance = eye.distance;
            hasNormalEyeDistance = true;
        }

        CacheOriginalMaterials();
    }

    private void Update()
    {
        base.UpdateAI();
        RecoverInk();
        UpdateOctopusBlackboard();
        UpdateRestSpeed();

        if (!isCamouflaging)
            RecuperarCamuflaje();

        if (!isCamouflaging || camouflage <= 0f)
        {
            RestoreVisibility();
            RestoreCamouflageVisual();
        }

        isCamouflaging = false;
    }

    public void Comer()
    {
        hunger = Mathf.Max(0, hunger - 30f);
        energy = Mathf.Min(maxEnergy, energy + 10f);
    }

    public bool HasPreyFoodNearby()
    {
        currentFoodPrey = FindClosestPreyFood();
        UpdateFoodPreyBlackboard();
        return currentFoodPrey != null;
    }

    public void BuscarPresaComida()
    {
        if (currentFoodPrey == null && !HasPreyFoodNearby())
            return;

        float distance = Vector3.Distance(transform.position, currentFoodPrey.position);
        if (distance <= preyEatRange)
        {
            ComerPresa();
            return;
        }

        OctopusVehicleLand octopusVehicle = survivorVehicle as OctopusVehicleLand;
        if (octopusVehicle != null)
            octopusVehicle.MoveToFoodPrey(currentFoodPrey);
    }

    public void ComerPresa()
    {
        if (currentFoodPrey == null)
            return;

        if (Time.time < lastPreyEatTime + preyEatCooldown)
            return;

        if (Vector3.Distance(transform.position, currentFoodPrey.position) > preyEatRange)
            return;

        HealthBase preyHealth = currentFoodPrey.GetComponentInParent<HealthBase>();
        if (preyHealth != null && !preyHealth.IsDead)
            preyHealth.ApplyDamage(preyEatDamage, WeaponType.Normal);

        hunger = Mathf.Clamp(hunger - 20f, 0f, maxHunger);
        lastPreyEatTime = Time.time;
        UpdateFoodPreyBlackboard();
    }

    public void Descansar()
    {
        isResting = true;
        ApplyRestSpeed();

        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (energy > UpEnergiaBaja + 10f)
        {
            RestoreOctopusNormalSpeed();
            isResting = false;
        }
    }

    public void Camuflarse()
    {
        if (camouflage <= 0f)
        {
            RestoreVisibility();
            RestoreCamouflageVisual();
            return;
        }

        isCamouflaging = true;
        camouflage = Mathf.Clamp(camouflage - CamouflageDrain * Time.deltaTime, 0f, maxCamouflage);
        ApplyCamouflageVisual();

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
        camouflage = Mathf.Clamp(camouflage + CamouflageRecover * Time.deltaTime, 0f, maxCamouflage);
    }

    private void CacheOriginalMaterials()
    {
        if (octopusRenderers != null)
            return;

        octopusRenderers = GetComponentsInChildren<Renderer>();
        octopusMaterials = new Material[octopusRenderers.Length][];
        originalMaterialColors = new Color[octopusRenderers.Length][];
        originalRenderQueues = new int[octopusRenderers.Length][];
        originalSurfaceValues = new float[octopusRenderers.Length][];
        originalBlendValues = new float[octopusRenderers.Length][];
        originalSrcBlendValues = new float[octopusRenderers.Length][];
        originalDstBlendValues = new float[octopusRenderers.Length][];
        originalZWriteValues = new float[octopusRenderers.Length][];
        originalTransparentKeywords = new bool[octopusRenderers.Length][];

        for (int i = 0; i < octopusRenderers.Length; i++)
        {
            octopusMaterials[i] = octopusRenderers[i].materials;
            originalMaterialColors[i] = new Color[octopusMaterials[i].Length];
            originalRenderQueues[i] = new int[octopusMaterials[i].Length];
            originalSurfaceValues[i] = new float[octopusMaterials[i].Length];
            originalBlendValues[i] = new float[octopusMaterials[i].Length];
            originalSrcBlendValues[i] = new float[octopusMaterials[i].Length];
            originalDstBlendValues[i] = new float[octopusMaterials[i].Length];
            originalZWriteValues[i] = new float[octopusMaterials[i].Length];
            originalTransparentKeywords[i] = new bool[octopusMaterials[i].Length];

            for (int j = 0; j < octopusMaterials[i].Length; j++)
            {
                Material material = octopusMaterials[i][j];
                originalMaterialColors[i][j] = GetMaterialColor(material);
                originalRenderQueues[i][j] = material.renderQueue;
                originalSurfaceValues[i][j] = GetMaterialFloat(material, "_Surface");
                originalBlendValues[i][j] = GetMaterialFloat(material, "_Blend");
                originalSrcBlendValues[i][j] = GetMaterialFloat(material, "_SrcBlend");
                originalDstBlendValues[i][j] = GetMaterialFloat(material, "_DstBlend");
                originalZWriteValues[i][j] = GetMaterialFloat(material, "_ZWrite");
                originalTransparentKeywords[i][j] = material.IsKeywordEnabled("_SURFACE_TYPE_TRANSPARENT");
            }
        }
    }

    private void ApplyCamouflageVisual()
    {
        CacheOriginalMaterials();

        if (isVisualCamouflaged)
            return;

        for (int i = 0; i < octopusMaterials.Length; i++)
        {
            for (int j = 0; j < octopusMaterials[i].Length; j++)
            {
                Material material = octopusMaterials[i][j];
                SetMaterialTransparent(material);

                Color color = GetMaterialColor(material);
                color.a = CamouflageAlpha;
                SetMaterialColor(material, color);
            }
        }

        isVisualCamouflaged = true;
    }

    private void RestoreCamouflageVisual()
    {
        if (!isVisualCamouflaged || octopusMaterials == null)
            return;

        for (int i = 0; i < octopusMaterials.Length; i++)
        {
            for (int j = 0; j < octopusMaterials[i].Length; j++)
            {
                Material material = octopusMaterials[i][j];
                SetMaterialColor(material, originalMaterialColors[i][j]);
                material.renderQueue = originalRenderQueues[i][j];
                SetMaterialFloat(material, "_Surface", originalSurfaceValues[i][j]);
                SetMaterialFloat(material, "_Blend", originalBlendValues[i][j]);
                SetMaterialFloat(material, "_SrcBlend", originalSrcBlendValues[i][j]);
                SetMaterialFloat(material, "_DstBlend", originalDstBlendValues[i][j]);
                SetMaterialFloat(material, "_ZWrite", originalZWriteValues[i][j]);

                if (originalTransparentKeywords[i][j])
                    material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                else
                    material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
        }

        isVisualCamouflaged = false;
    }

    private Color GetMaterialColor(Material material)
    {
        if (material.HasProperty("_BaseColor"))
            return material.GetColor("_BaseColor");

        if (material.HasProperty("_Color"))
            return material.GetColor("_Color");

        return Color.white;
    }

    private float GetMaterialFloat(Material material, string property)
    {
        return material.HasProperty(property) ? material.GetFloat(property) : 0f;
    }

    private void SetMaterialColor(Material material, Color color)
    {
        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);
    }

    private void SetMaterialFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property))
            material.SetFloat(property, value);
    }

    private void SetMaterialTransparent(Material material)
    {
        if (material.HasProperty("_Surface"))
            material.SetFloat("_Surface", 1f);

        if (material.HasProperty("_Blend"))
            material.SetFloat("_Blend", 0f);

        if (material.HasProperty("_SrcBlend"))
            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);

        if (material.HasProperty("_DstBlend"))
            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

        if (material.HasProperty("_ZWrite"))
            material.SetFloat("_ZWrite", 0f);

        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
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

        if (survivorVehicle != null)
            survivorVehicle.EvadeEnemy();
    }

    private void UpdateOctopusBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("Camouflage", camouflage);
        blackboard.SetFloat("Ink", ink);
        blackboard.SetBool("CamuflajeDisponible", camouflage > 0f);
        blackboard.SetBool("PuedeLiberarTinta", PuedeLiberarTinta());
        UpdateFoodPreyBlackboard();
    }

    private Transform FindClosestPreyFood()
    {
        int preyLayer = LayerMask.NameToLayer("Prey");
        if (preyLayer < 0)
            return null;

        Collider[] preyTargets = Physics.OverlapSphere(transform.position, preyFoodRange, 1 << preyLayer);
        Transform closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < preyTargets.Length; i++)
        {
            Transform prey = preyTargets[i].transform;
            HealthBase preyHealth = prey.GetComponentInParent<HealthBase>();
            if (preyHealth == null || preyHealth.IsDead)
                continue;

            float distance = Vector3.Distance(transform.position, prey.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = prey;
            }
        }

        return closest;
    }

    private void UpdateFoodPreyBlackboard()
    {
        if (blackboard == null)
            return;

        bool hasFoodPrey = currentFoodPrey != null;
        float distance = hasFoodPrey
            ? Vector3.Distance(transform.position, currentFoodPrey.position)
            : float.MaxValue;

        blackboard.SetBool("PresaComidaCerca", hasFoodPrey);
        blackboard.SetFloat("DistanciaPresaComida", distance);
    }

    private void RestoreVisibility()
    {
        if (eye != null && hasNormalEyeDistance)
            eye.distance = normalEyeDistance;
    }

    private void ApplyRestSpeed()
    {
        if (survivorVehicle != null)
            survivorVehicle.SetSpeedMultiplier(0.4f);
    }

    private void UpdateRestSpeed()
    {
        if (!isResting || energy > UpEnergiaBaja + 10f)
            RestoreOctopusNormalSpeed();

        isResting = false;
    }

    private void RestoreOctopusNormalSpeed()
    {
        if (survivorVehicle != null)
            survivorVehicle.RestoreNormalSpeed();
    }
}
