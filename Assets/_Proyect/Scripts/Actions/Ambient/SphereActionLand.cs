using UnityEngine;

public class SphereActionLand : AICharacterAction
{
    [Header("Sphere Settings")]
    public float intensity = 100f;
    public float influenceRadius = 20f;
    public float pulseInterval = 5f;
    public float stability = 100f;
    public ParticleSystem pulseParticlePrefab;

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
        UpdateSphereBlackboard();
    }

    public bool CanPulse()
    {
        return energy >= 20f && Time.time >= lastPulseTime + pulseInterval;
    }

    public void EmitirPulso()
    {
        if (!CanPulse())
            return;

        energy = Mathf.Clamp(energy - 20f, 0f, 100f);
        lastPulseTime = Time.time;
        pulseActive = true;

        AtraerPeces();
        RepelerDepredadores();
        ReducirIntensidad();

        if (pulseParticlePrefab != null)
        {
            ParticleSystem pulseEffect = Instantiate(pulseParticlePrefab, transform.position, Quaternion.identity);
            pulseEffect.Play();
            Destroy(pulseEffect.gameObject, 2f);
        }
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
                preyVehicle.ArriveBehaviour(transform.position, influenceRadius);
        }
    }

    public void RepelerDepredadores()
    {
        Collider[] predatorObjects = Physics.OverlapSphere(transform.position, influenceRadius, LayerMask.GetMask("Predator"));
        nearbyPredatorCount = predatorObjects.Length;
        pulseActive = pulseActive || nearbyPredatorCount > 0;

        for (int i = 0; i < predatorObjects.Length; i++)
        {
            AICharacterVehicle predatorVehicle = predatorObjects[i].GetComponentInParent<AICharacterVehicle>();
            if (predatorVehicle != null)
                predatorVehicle.FleeBehaviour(transform.position);
        }
    }

    public void ReducirIntensidad()
    {
        if (energy < 20f)
            intensity = Mathf.Clamp(intensity - 10f * Time.deltaTime, 0f, 100f);
    }

    private void UpdateSphereBlackboard()
    {
        ReducirIntensidad();

        if (blackboard == null)
            return;

        blackboard.SetFloat("Intensity", intensity);
        blackboard.SetFloat("InfluenceRadius", influenceRadius);
        blackboard.SetFloat("PulseInterval", pulseInterval);
        blackboard.SetFloat("Energy", energy);
        blackboard.SetFloat("Stability", stability);
        blackboard.SetBool("PuedeEmitirPulso", CanPulse());
        blackboard.SetBool("PulseActive", pulseActive);
        blackboard.SetFloat("NearbyPrey", nearbyPreyCount);
        blackboard.SetFloat("NearbyPredators", nearbyPredatorCount);
    }
}
