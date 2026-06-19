using UnityEngine;

//public abstract class SurvivorActionLand : AICharacterAction
//{
//    [Header("Survivor Settings")]
//    public float camouflage = 50f;
//    public float maxCamouflage = 100f;

//    public virtual void Hide()
//    {
//        var hide = GetComponent<Hide>();
//        if (hide != null)
//        {
//            Transform threat = eye.GetNearestTarget();
//            if (threat != null)
//            {
//                hide.SetThreat(threat);
//                hide.isActive = true;
//                steering.AddBehavior(hide);
//            }
//        }
//    }

//    public virtual void ActivateCamouflage()
//    {
//        Renderer renderer = GetComponent<Renderer>();
//        if (renderer != null)
//        {
//            renderer.material.SetFloat("_Opacity", 0.1f);
//        }
//        camouflage -= Time.deltaTime * 2f;
//        steering.maxSpeed = 0.5f;
//    }
//}