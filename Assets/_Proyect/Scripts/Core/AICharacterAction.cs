using UnityEngine;

public abstract class AICharacterAction : AICharacterControl
{
    [Header("Action Settings")]
    public float actionRadius = 10f;
    public LayerMask targetLayer;
    [Header("Action Settings")]
    public float hunger = 50f;
    public float Scalehunger = 1.5f;
    public float Scaleenergy = 0.5f;
    public float maxHunger = 100f;
    public float energy = 100f;
    public float maxEnergy = 100f;

    public float UpHambreAlta = 70;
    public float UpHambreMedia = 30;
    public float UpEnergiaBaja = 30;
    public override void LoadComponent()
    {
        base.LoadComponent();

    }
    protected void BlackboardHungerEnergy()
    {
        hunger = Mathf.Clamp(hunger + Scalehunger * Time.deltaTime, 0, maxHunger);
        energy = Mathf.Clamp(energy - Scaleenergy * Time.deltaTime, 0, maxEnergy);
        if (blackboard != null)
        {
            blackboard.SetFloat("Hunger", hunger);
            blackboard.SetFloat("Energy", energy);
            blackboard.SetBool("HambreAlta", hunger > UpHambreAlta);
            blackboard.SetBool("HambreMedia", hunger > UpHambreMedia && hunger <= UpHambreAlta);
            blackboard.SetBool("EnergiaBaja", energy < UpEnergiaBaja);
            blackboard.SetBool("VePresa", eye != null && eye.ViewEnemy != null);
        }
    }
    protected void BlackboardVePresa()
    {
        if (blackboard != null)
        {
            blackboard.SetBool("VePresa", eye != null && eye.ViewEnemy != null);
        }
    }
    public virtual void UpdateAI()
    {
        BlackboardHungerEnergy();
        BlackboardVePresa();
    }
   
}