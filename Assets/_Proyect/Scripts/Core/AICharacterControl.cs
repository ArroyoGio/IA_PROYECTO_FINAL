using UnityEngine;

public abstract class AICharacterControl : MonoBehaviour
{
    [Header("References")]
    public SteeringManager steering;
    public HealthBase health;
    public Blackboard blackboard;
    public AIEyeBase eye;

    protected virtual void Awake()
    {
        steering = GetComponent<SteeringManager>();
        health = GetComponent<HealthBase>();
        blackboard = GetComponent<Blackboard>();
        eye = GetComponent<AIEyeBase>();
    }

    public abstract void UpdateAI();
}