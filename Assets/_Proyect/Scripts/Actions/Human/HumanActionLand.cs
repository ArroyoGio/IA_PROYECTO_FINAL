using UnityEngine;

//public abstract class HumanActionLand : AICharacterAction
//{
//    [Header("Human Settings")]
//    public float energy = 100f;
//    public float maxEnergy = 100f;

//    public virtual void Explore()
//    {
//        var wander = GetComponent<Wander>();
//        if (wander != null)
//        {
//            wander.isActive = true;
//            steering.AddBehavior(wander);
//        }
//    }

//    public virtual void Rest()
//    {
//        energy += 10f * Time.deltaTime;
//        energy = Mathf.Clamp(energy, 0, maxEnergy);
//        steering.Stop();
//    }
//}