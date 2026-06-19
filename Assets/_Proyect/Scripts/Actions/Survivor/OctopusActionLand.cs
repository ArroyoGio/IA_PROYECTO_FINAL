using UnityEngine;

//public class OctopusActionLand : SurvivorActionLand
//{
//    [Header("Octopus Settings")]
//    public float hunger = 50f;
//    public float maxHunger = 100f;
//    public float fear = 0f;
//    public float maxFear = 100f;
//    public float ink = 100f;
//    public float maxInk = 100f;

//    private Wander wander;
//    private Hide hide;
//    private Evade evade;

//    protected override void Awake()
//    {
//        base.Awake();
//        wander = GetComponent<Wander>();
//        hide = GetComponent<Hide>();
//        evade = GetComponent<Evade>();
//    }

//    // TODA la lógica va AQUÍ
//    public override void UpdateAI()
//    {
//        hunger += 1f * Time.deltaTime;
//        hunger = Mathf.Clamp(hunger, 0, maxHunger);

//        camouflage += 2f * Time.deltaTime;
//        camouflage = Mathf.Clamp(camouflage, 0, maxCamouflage);

//        ink += 1f * Time.deltaTime;
//        ink = Mathf.Clamp(ink, 0, maxInk);

//        if (eye.HasTargets())
//            fear += 8f * Time.deltaTime;
//        else
//            fear -= 2f * Time.deltaTime;
//        fear = Mathf.Clamp(fear, 0, maxFear);

//        if (blackboard != null)
//        {
//            blackboard.SetFloat("Hunger", hunger);
//            blackboard.SetFloat("Camouflage", camouflage);
//            blackboard.SetFloat("Fear", fear);
//            blackboard.SetFloat("Ink", ink);
//            blackboard.SetBool("HambreAlta", hunger > 70f);
//            blackboard.SetBool("CamuflajeDisponible", camouflage > 20f);
//            blackboard.SetBool("CamuflajeBajo", camouflage < 20f);
//            blackboard.SetBool("MiedoAlto", fear > 80f);
//            blackboard.SetBool("VeAmenaza", eye.HasTargets());
//        }
//    }

//    public void ActivarCamuflaje()
//    {
//        base.ActivateCamouflage();
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

//    public void RecargarCamuflaje()
//    {
//        camouflage += 10f * Time.deltaTime;
//        camouflage = Mathf.Clamp(camouflage, 0, maxCamouflage);
//    }

//    public void LiberarTinta()
//    {
//        if (ink >= 30f)
//        {
//            ink -= 30f;
//            ParticleSystem inkEffect = GetComponent<ParticleSystem>();
//            if (inkEffect != null)
//                inkEffect.Play();
//            Evade();
//        }
//    }

//    public void BuscarComida()
//    {
//        if (wander != null)
//        {
//            wander.isActive = true;
//            steering.AddBehavior(wander);
//        }

//        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
//        if (foodItems.Length > 0)
//        {
//            Transform closest = null;
//            float closestDist = float.MaxValue;
//            foreach (var food in foodItems)
//            {
//                float dist = Vector3.Distance(transform.position, food.transform.position);
//                if (dist < closestDist)
//                {
//                    closestDist = dist;
//                    closest = food.transform;
//                }
//            }
//            if (closest != null)
//                steering.SetTarget(closest.position);
//        }
//    }

//    public void Comer() => hunger = Mathf.Max(0, hunger - 25f);

//    public void Evade()
//    {
//        if (evade != null)
//        {
//            Transform threat = eye.GetNearestTarget();
//            if (threat != null)
//            {
//                evade.SetThreat(threat);
//                evade.isActive = true;
//                steering.AddBehavior(evade);
//            }
//        }
//    }

//    public override void PerformAction() { }
//}