using UnityEngine;

public abstract class SurvivorActionLand : AICharacterAction
{
    [Header("Survivor Settings")]
    public float threat;
    public float hiding;

    protected SurvivorVehicleLand survivorVehicle;
    protected Transform currentPrey;
    protected Transform currentThreat;

    protected const float ThreatHighValue = 100f;
    protected const float ThreatDecayRate = 35f;

    public override void LoadComponent()
    {
        base.LoadComponent();
        survivorVehicle = GetComponent<SurvivorVehicleLand>();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateSurvivorState();
        UpdateSurvivorBlackboard();
    }

    public virtual bool IsPreyClose()
    {
        currentPrey = GetDetectedTargetOnLayer("Prey");
        return currentPrey != null;
    }

    public virtual bool HasPredatorThreat()
    {
        currentThreat = GetDetectedTargetOnLayer("Predator");
        return currentThreat != null;
    }

    protected virtual void UpdateSurvivorState()
    {
        bool seesThreat = HasPredatorThreat();
        threat = seesThreat
            ? ThreatHighValue
            : Mathf.Clamp(threat - ThreatDecayRate * Time.deltaTime, 0f, 100f);

        if (threat <= 60f)
            hiding = Mathf.Clamp(hiding - 60f * Time.deltaTime, 0f, 100f);
    }

    protected virtual void UpdateSurvivorBlackboard()
    {
        if (blackboard == null)
            return;

        bool preyClose = IsPreyClose();
        bool seesThreat = currentThreat != null;

        blackboard.SetFloat("Threat", threat);
        blackboard.SetBool("Hiding", hiding > 0f);
        blackboard.SetBool("AmenazaAlta", seesThreat && threat > 60f);
        blackboard.SetBool("PresaCerca", preyClose);

        if (currentPrey != null)
            blackboard.SetObject("PreyTarget", currentPrey);
    }

    protected Transform GetDetectedTargetOnLayer(string layerName)
    {
        if (eye == null || eye.ViewEnemy == null)
            return null;

        Transform target = eye.ViewEnemy.transform;
        return IsInLayerHierarchy(target, layerName) ? target : null;
    }

    protected bool IsInLayerHierarchy(Transform target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0 || target == null)
            return false;

        Transform current = target;
        while (current != null)
        {
            if (current.gameObject.layer == layer)
                return true;

            current = current.parent;
        }

        Transform[] children = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.layer == layer)
                return true;
        }

        return false;
    }
}
