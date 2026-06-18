using UnityEngine;

public class FishActionLand : PreyActionLand
{
    [Header("Fish Settings")]
    public float hunger = 0f;
    public float maxHunger = 100f;
    public float energy = 100f;
    public float maxEnergy = 100f;
    public float hungerRate = 2f;
    public float energyConsumption = 1f;

    public Transform schoolLeader;
    public float schoolDistance = 3f;

    private Wander wander;
    private Evade evade;
    private OffsetPursuit offsetPursuit;

    protected override void Awake()
    {
        base.Awake();
        wander = GetComponent<Wander>();
        evade = GetComponent<Evade>();
        offsetPursuit = GetComponent<OffsetPursuit>();
    }

    public override void UpdateAI()
    {
        // Actualizar variables
        hunger += hungerRate * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        energy -= energyConsumption * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        // Actualizar miedo
        if (eye.HasTargets())
        {
            fear += 10f * Time.deltaTime;
        }
        else
        {
            fear -= 2f * Time.deltaTime;
        }
        fear = Mathf.Clamp(fear, 0, maxFear);

        // Actualizar Blackboard
        if (blackboard != null)
        {
            blackboard.SetFloat("Hunger", hunger);
            blackboard.SetFloat("Energy", energy);
            blackboard.SetFloat("Fear", fear);
            blackboard.SetBool("HambreAlta", hunger > 70f);
            blackboard.SetBool("EnergiaBaja", energy < 30f);
            blackboard.SetBool("MiedoAlto", fear > 70f);
            blackboard.SetBool("VeDepredador", eye.HasTargets());
        }
    }

    public void MantenerCardumen()
    {
        if (schoolLeader != null && offsetPursuit != null)
        {
            offsetPursuit.SetLeader(schoolLeader);
            Vector3 randomOffset = Random.insideUnitSphere * schoolDistance;
            randomOffset.y = 0;
            offsetPursuit.SetOffset(randomOffset);
            offsetPursuit.isActive = true;
            steering.AddBehavior(offsetPursuit);
        }
    }

    public void BuscarComida()
    {
        if (wander != null)
        {
            wander.isActive = true;
            steering.AddBehavior(wander);
        }

        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
        if (foodItems.Length > 0)
        {
            Transform closest = null;
            float closestDist = float.MaxValue;
            foreach (var food in foodItems)
            {
                float dist = Vector3.Distance(transform.position, food.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = food.transform;
                }
            }

            if (closest != null)
            {
                steering.SetTarget(closest.position);
            }
        }
    }

    public void Comer()
    {
        hunger = Mathf.Max(0, hunger - 30f);
        energy = Mathf.Min(maxEnergy, energy + 10f);
    }

    public new void Evade()
    {
        base.Evade();
        energy -= 2f * Time.deltaTime;
    }

    public void Descansar()
    {
        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (wander != null)
        {
            wander.isActive = false;
        }
        steering.Stop();
    }

    public override void PerformAction()
    {
        // Este método se llama desde el Behavior Tree
    }
}