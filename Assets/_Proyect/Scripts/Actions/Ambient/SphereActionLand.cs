using UnityEngine;

public class SphereActionLand : AICharacterAction
{
    private const float LowEnergyThreshold = 25f;
    private const float PulseVisualDuration = 0.8f;

    [Header("Sphere Settings")]
    public float intensity = 100f;
    public float influenceRadius = 20f;
    public float pulseInterval = 5f;
    public float stability = 100f;
    public ParticleSystem pulseParticlePrefab;
    public float pulseEnergyCost = 20f;
    public float energyRecoverRate = 8f;
    public float attractionForce = 3f;
    public float repelForce = 20f;
    public float repelDuration = 1.5f;

    private float lastPulseTime = -999f;
    private int nearbyPreyCount;
    private int nearbyPredatorCount;
    private bool pulseActive;

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
        RecoverEnergy();
        ReducirIntensidad();
        UpdateSphereBlackboard();
    }

    public bool CanPulse()
    {
        return energy >= pulseEnergyCost && Time.time >= lastPulseTime + pulseInterval;
    }

    public void EmitirPulso()
    {
        if (!CanPulse())
            return;

        energy = Mathf.Clamp(energy - pulseEnergyCost, 0f, maxEnergy);
        lastPulseTime = Time.time;
        pulseActive = true;

        CreatePulseVisual();
        AtraerPeces();
        RepelerDepredadores();
        ReducirIntensidad();
    }

    public void AtraerPeces()
    {
        Collider[] preyObjects = Physics.OverlapSphere(transform.position, influenceRadius, LayerMask.GetMask("Prey"));
        nearbyPreyCount = preyObjects.Length;
        pulseActive = nearbyPreyCount > 0;

        for (int i = 0; i < preyObjects.Length; i++)
        {
            AICharacterVehicle preyVehicle = preyObjects[i].GetComponentInParent<AICharacterVehicle>();
            if (preyVehicle != null)
                preyVehicle.ArriveBehaviour(transform.position, attractionForce);
        }
    }

    public void RepelerDepredadores()
    {
        Collider[] predatorObjects = Physics.OverlapSphere(transform.position, influenceRadius, LayerMask.GetMask("Predator"));
        nearbyPredatorCount = predatorObjects.Length;
        pulseActive = pulseActive || nearbyPredatorCount > 0;

        for (int i = 0; i < predatorObjects.Length; i++)
        {
            Collider predatorCollider = predatorObjects[i];
            AICharacterVehicle predatorVehicle = predatorCollider.GetComponentInParent<AICharacterVehicle>();
            if (predatorVehicle == null)
                predatorVehicle = predatorCollider.GetComponentInChildren<AICharacterVehicle>();

            if (predatorVehicle != null)
                StartCoroutine(RepelForSeconds(predatorVehicle, repelDuration));
        }
    }

    public void ReducirIntensidad()
    {
        if (energy < LowEnergyThreshold)
            intensity = Mathf.Clamp(intensity - 10f * Time.deltaTime, 0f, 100f);
    }

    private void RecoverEnergy()
    {
        energy = Mathf.Clamp(energy + energyRecoverRate * Time.deltaTime, 0f, maxEnergy);
    }

    private System.Collections.IEnumerator RepelForSeconds(AICharacterVehicle predatorVehicle, float duration)
    {
        float elapsed = 0f;
        Transform target = predatorVehicle.transform;

        while (elapsed < duration && target != null)
        {
            predatorVehicle.Stop();

            Vector3 away = target.position - transform.position;
            away.y = 0f;

            if (away.sqrMagnitude > 0.01f)
                target.position += away.normalized * repelForce * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void CreatePulseVisual()
    {
        if (pulseParticlePrefab != null)
        {
            ParticleSystem pulseEffect = Instantiate(pulseParticlePrefab, transform.position, Quaternion.identity);
            pulseEffect.Play();
            Destroy(pulseEffect.gameObject, 2f);
            return;
        }

        GameObject pulseSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pulseSphere.transform.position = transform.position;
        pulseSphere.transform.localScale = Vector3.one * influenceRadius * 2f;

        Renderer pulseRenderer = pulseSphere.GetComponent<Renderer>();
        if (pulseRenderer != null)
        {
            Material pulseMaterial = new Material(Shader.Find("Standard"));
            pulseMaterial.color = new Color(0.2f, 0.9f, 1f, 0.25f);
            pulseMaterial.SetFloat("_Mode", 3f);
            pulseMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            pulseMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            pulseMaterial.SetInt("_ZWrite", 0);
            pulseMaterial.DisableKeyword("_ALPHATEST_ON");
            pulseMaterial.EnableKeyword("_ALPHABLEND_ON");
            pulseMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            pulseMaterial.renderQueue = 3000;
            pulseRenderer.material = pulseMaterial;
            Destroy(pulseMaterial, PulseVisualDuration);
        }

        Collider pulseCollider = pulseSphere.GetComponent<Collider>();
        if (pulseCollider != null)
            Destroy(pulseCollider);

        Destroy(pulseSphere, PulseVisualDuration);
    }

    private void UpdateSphereBlackboard()
    {
        if (blackboard == null)
            return;

        bool pulseReady = CanPulse();
        blackboard.SetFloat("Intensity", intensity);
        blackboard.SetFloat("InfluenceRadius", influenceRadius);
        blackboard.SetFloat("PulseInterval", pulseInterval);
        blackboard.SetFloat("Energy", energy);
        blackboard.SetFloat("Stability", stability);
        blackboard.SetBool("PulseReady", pulseReady);
        blackboard.SetBool("PuedeEmitirPulso", pulseReady);
        blackboard.SetBool("PulseActive", pulseActive);
        blackboard.SetFloat("NearbyPrey", nearbyPreyCount);
        blackboard.SetFloat("NearbyPredators", nearbyPredatorCount);
    }
}
