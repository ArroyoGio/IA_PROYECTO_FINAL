using UnityEngine;
using UnityEngine.AI;

public abstract class AICharacterControl : MonoBehaviour
{
    [Header("Core References")]
    protected HealthBase health;
    protected Blackboard blackboard;
    protected AIEye eye;
     
    public HealthBase Health => health;
    public Blackboard Blackboard => blackboard;
    public AIEye Eye => eye;
 
    public virtual void LoadComponent()
    {
         if(health==null)
            health = GetComponent<HealthBase>();
         if(eye==null)
            eye = GetComponent<AIEye>();
         if(blackboard==null)
            blackboard = GetComponent<Blackboard>();
    }

    public HealthBase GetHealth() => health;
    public Blackboard GetBlackboard() => blackboard;
    public AIEye GetEye() => eye;
 
}