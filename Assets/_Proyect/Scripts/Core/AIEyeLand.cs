using UnityEngine;
using System.Collections.Generic;

public class AIEyeLand : AIEyeBase
{
    [Header("Detection Tags")]
    public string preyTag = "Prey";
    public string predatorTag = "Predator";
    public string foodTag = "Food";

    protected override void UpdateBlackboard()
    {
        if (blackboard == null)
            blackboard = GetComponent<Blackboard>();

        if (blackboard == null) return;

        Transform nearestPrey = GetNearestTargetByTag(preyTag);
        Transform nearestPredator = GetNearestTargetByTag(predatorTag);
        Transform nearestFood = GetNearestTargetByTag(foodTag);

        blackboard.SetObject("NearestPrey", nearestPrey);
        blackboard.SetObject("NearestPredator", nearestPredator);
        blackboard.SetObject("NearestFood", nearestFood);

        blackboard.SetBool("HasPrey", nearestPrey != null);
        blackboard.SetBool("HasPredator", nearestPredator != null);
        blackboard.SetBool("HasFood", nearestFood != null);

        if (nearestPrey != null)
        {
            blackboard.SetFloat("PreyDistance", Vector3.Distance(transform.position, nearestPrey.position));
        }

        if (nearestPredator != null)
        {
            blackboard.SetFloat("PredatorDistance", Vector3.Distance(transform.position, nearestPredator.position));
        }

        if (nearestFood != null)
        {
            blackboard.SetFloat("FoodDistance", Vector3.Distance(transform.position, nearestFood.position));
        }
    }

    private Transform GetNearestTargetByTag(string tag)
    {
        Transform nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (var target in visibleTargets)
        {
            if (target.CompareTag(tag))
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = target;
                }
            }
        }

        return nearest;
    }
}

