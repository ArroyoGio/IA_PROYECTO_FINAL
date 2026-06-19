using UnityEngine;

//public class SphereActionLand : AmbientActionLand
//{
//    [Header("Sphere Settings")]
//    public float intensity = 50f;
//    public float maxIntensity = 100f;
//    public float radius = 15f;
//    public float maxRadius = 20f;
//    public float cycleTime = 10f;
//    public float attractionForce = 5f;
//    public float repulsionForce = 8f;

//    private Light sphereLight;
//    private ParticleSystem particles;
//    private float cycleTimer = 0f;

//    protected override void Awake()
//    {
//        base.Awake();
//        sphereLight = GetComponent<Light>();
//        particles = GetComponent<ParticleSystem>();
//    }

//    public override void UpdateAI()
//    {
//        cycleTimer += Time.deltaTime;
//        timer += Time.deltaTime;

//        if (timer >= pulseInterval)
//        {
//            EmitPulse();
//        }

//        if (blackboard != null)
//        {
//            blackboard.SetFloat("Intensity", intensity);
//            blackboard.SetFloat("Radius", radius);
//            blackboard.SetFloat("Cycle", cycleTimer);
//        }

//        PulseCycle();
//    }

//    public void CalcularIntensidad()
//    {
//        intensity = 50f + 50f * Mathf.Sin(cycleTimer * (2f * Mathf.PI / cycleTime));
//        intensity = Mathf.Clamp(intensity, 10f, maxIntensity);

//        radius = 10f + intensity / 10f;
//        radius = Mathf.Clamp(radius, 5f, maxRadius);

//        if (sphereLight != null)
//        {
//            sphereLight.intensity = intensity / 20f;
//        }
//    }

//    public new void EmitPulse()
//    {
//        base.EmitPulse();

//        if (particles != null)
//        {
//            particles.Emit((int)intensity / 5);
//        }

//        timer = 0f;
//    }

//    public void AtraerPeces()
//    {
//        Collider[] fish = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Fish"));
//        foreach (var fishObj in fish)
//        {
//            FishActionLand fishAction = fishObj.GetComponent<FishActionLand>();
//            if (fishAction != null)
//            {
//                Vector3 direction = (transform.position - fishObj.transform.position).normalized;
//                fishAction.steering.AddForce(direction * attractionForce * (intensity / 100f));
//                fishAction.fear = Mathf.Max(0, fishAction.fear - 5f * Time.deltaTime);
//            }
//        }
//    }

//    public void RepelerDepredadores()
//    {
//        Collider[] predators = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Shark", "Snake"));
//        foreach (var predator in predators)
//        {
//            PredatorActionLand predatorAction = predator.GetComponent<PredatorActionLand>();
//            if (predatorAction != null)
//            {
//                Vector3 direction = (predator.transform.position - transform.position).normalized;
//                predatorAction.steering.AddForce(direction * repulsionForce * (intensity / 100f));
//            }
//        }
//    }

//    public void PulseCycle()
//    {
//        CalcularIntensidad();
//        EmitPulse();
//        AtraerPeces();
//        RepelerDepredadores();
//    }

//    public override void PerformAction()
//    {
//        PulseCycle();
//    }

//    // ============================================
//    // MÉTODOS PÚBLICOS PARA EL BEHAVIOR TREE (ESPAÑOL)
//    // ============================================

//    public void EmitirPulso()
//    {
//        EmitPulse();
//    }
//}