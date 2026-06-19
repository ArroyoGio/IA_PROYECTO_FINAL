using UnityEngine;

public abstract class PreyActionLand : AICharacterAction
{
    [Header("Prey Settings")]
    public float fear = 0f;
    public float maxFear = 100f;
    public float DownMiedoAlto = 100f;
    void Awake()
    {
        LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();

    }
    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateFear();
    }
    protected void UpdateFear()
            {
        if (eye.ViewEnemy)
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
            blackboard.SetFloat("Fear", fear);
            blackboard.SetBool("MiedoAlto", fear > DownMiedoAlto);
            blackboard.SetBool("VeDepredador", eye.ViewEnemy!=null);
        }
    }
}