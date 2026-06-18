using UnityEngine;

public class DolphinActionLand : PreyActionLand
{
    [Header("Dolphin Settings")]
    public float energy = 100f;
    public float maxEnergy = 100f;
    public float air = 100f;
    public float maxAir = 100f;
    public float airConsumption = 2f;
    public float energyConsumption = 1f;
    public float jumpForce = 10f;

    private Wander wander;
    private EvadeNode evade;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        wander = GetComponent<Wander>();
        evade = GetComponent<EvadeNode>();
        rb = GetComponent<Rigidbody>();
    }

    //TODA la lógica va AQUÍ
    public override void UpdateAI()
    {
        air -= airConsumption * Time.deltaTime;
        air = Mathf.Clamp(air, 0, maxAir);

        energy -= energyConsumption * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (eye.HasTargets())
        {
            fear += 8f * Time.deltaTime;
        }
        else
        {
            fear -= 2f * Time.deltaTime;
        }
        fear = Mathf.Clamp(fear, 0, maxFear);

        if (blackboard != null)
        {
            blackboard.SetFloat("Energy", energy);
            blackboard.SetFloat("Air", air);
            blackboard.SetFloat("Fear", fear);
            blackboard.SetBool("EnergiaBaja", energy < 30f);
            blackboard.SetBool("AireBajo", air < 30f);
            blackboard.SetBool("MiedoAlto", fear > 70f);
            blackboard.SetBool("VeDepredador", eye.HasTargets());
            blackboard.SetBool("EsferaCercana", IsSphereNear());
        }
    }

    bool IsSphereNear()
    {
        GameObject sphere = GameObject.FindGameObjectWithTag("Sphere");
        if (sphere != null)
        {
            return Vector3.Distance(transform.position, sphere.transform.position) < 10f;
        }
        return false;
    }

    public void Saltar()
    {
        if (rb != null && transform.position.y < 2f)
        {
            Vector3 jumpDirection = (transform.forward * 2f + Vector3.up * 5f).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            energy -= 10f;
        }
    }

    public void IrSuperficie()
    {
        Vector3 surfacePosition = new Vector3(transform.position.x, 0f, transform.position.z);
        steering.SetTarget(surfacePosition);
        air += 20f * Time.deltaTime;
        air = Mathf.Clamp(air, 0, maxAir);
    }

    public void NadarLento()
    {
        if (wander != null)
        {
            wander.isActive = true;
            steering.AddBehavior(wander);
        }
        steering.maxSpeed = 2f;
    }

    public void JugarConEsfera()
    {
        GameObject sphere = GameObject.FindGameObjectWithTag("Sphere");
        if (sphere != null)
        {
            Vector3 orbitPosition = sphere.transform.position + Random.insideUnitSphere * 5f;
            orbitPosition.y = 0;
            steering.SetTarget(orbitPosition);
            energy -= 2f * Time.deltaTime;
        }
    }

    public new void Evade()
    {
        base.Evade();
        energy -= 3f * Time.deltaTime;
    }

    public override void PerformAction() { }
}