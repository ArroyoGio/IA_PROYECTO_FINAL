using UnityEngine;

public class SharkActionLand : PredatorActionLand
{
    [Header("Shark Settings")]
    public float hunger = 50f;
    public float maxHunger = 100f;
    public float energy = 100f;
    public float maxEnergy = 100f;
    public float aggressiveness = 50f;
    public float huntingRange = 20f;

    private Wander wander;
    private Pursuit pursuit;

    protected override void Awake()
    {
        base.Awake();
        wander = GetComponent<Wander>();
        pursuit = GetComponent<Pursuit>();
    }

    // TODA la lógica va AQUÍ
    public override void UpdateAI()
    {
        hunger += 1.5f * Time.deltaTime;
        energy -= 0.5f * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (blackboard != null)
        {
            blackboard.SetFloat("Hunger", hunger);
            blackboard.SetFloat("Energy", energy);
            blackboard.SetBool("HambreAlta", hunger > 70f);
            blackboard.SetBool("HambreMedia", hunger > 30f && hunger <= 70f);
            blackboard.SetBool("EnergiaBaja", energy < 30f);
            blackboard.SetBool("VePresa", eye.HasTargets());
            blackboard.SetBool("PresaAlcance", IsTargetInRange(eye.GetNearestTarget()));
        }
    }

    public void Cazar()
    {
        Transform prey = eye.GetNearestTarget();
        if (prey != null)
        {
            if (prey.CompareTag("Fish") || prey.CompareTag("Dolphin"))
            {
                Pursue(prey);
            }
        }
    }

    public void Morder()
    {
        Transform prey = eye.GetNearestTarget();
        Attack(prey);
        if (prey != null)
        {
            hunger = Mathf.Max(0, hunger - 30f);
            energy = Mathf.Max(0, energy - 5f);
            aggressiveness = Mathf.Min(100, aggressiveness + 5f);
        }
    }

    public void Patrullar()
    {
        if (wander != null)
        {
            wander.isActive = true;
            steering.AddBehavior(wander);
        }
    }

    public void Descansar()
    {
        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        steering.Stop();
    }

    public override void PerformAction() { }
}