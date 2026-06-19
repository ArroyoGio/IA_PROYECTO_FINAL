//using UnityEngine;

//public class SnakeActionLand : PredatorActionLand
//{
   
//    public float bioEnergy = 100f;
//    public float maxBioEnergy = 100f;
//    public float stealth = 50f;
//    public float maxStealth = 100f;
//    public float flashCost = 20f;



//    void Awake()
//    {
//        LoadComponent();

//    }
//    public override void LoadComponent()
//    {
//        base.LoadComponent();

//    }
//    private void Update()
//    {
//        base.UpdateAI();

//    }

//    //TODA la lógica va AQUÍ
//    public override void UpdateAI()
//    {
//        hunger += 1.2f * Time.deltaTime;
//        hunger = Mathf.Clamp(hunger, 0, maxHunger);

//        bioEnergy += 0.5f * Time.deltaTime;
//        bioEnergy = Mathf.Clamp(bioEnergy, 0, maxBioEnergy);

//        if (blackboard != null)
//        {
//            blackboard.SetFloat("Hunger", hunger);
//            blackboard.SetFloat("BioEnergy", bioEnergy);
//            blackboard.SetFloat("Stealth", stealth);
//            blackboard.SetBool("HambreAlta", hunger > 70f);
//            blackboard.SetBool("EnergiaBioluminiscenteAlta", bioEnergy > 30f);
//            blackboard.SetBool("EnergiaBioluminiscenteBaja", bioEnergy < 20f);
//            blackboard.SetBool("VePresa", eye.HasTargets());
//            blackboard.SetBool("PresaAlcance", IsTargetInRange(eye.GetNearestTarget()));
//        }
//    }

//    public void EmitirDestello()
//    {
//        if (bioEnergy >= flashCost)
//        {
//            bioEnergy -= flashCost;

//            Collider[] targets = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Fish", "Dolphin"));
//            foreach (var target in targets)
//            {
//                var prey = target.GetComponent<PreyActionLand>();
//                if (prey != null)
//                {
//                    prey.fear = Mathf.Min(100, prey.fear + 30f);
//                }
//            }
//        }
//    }

//    public void Constreñir()
//    {
//        Transform prey = eye.GetNearestTarget();
//        if (prey != null && Vector3.Distance(transform.position, prey.position) < attackRange)
//        {
//            HealthBase health = prey.GetComponent<HealthBase>();
//            if (health != null)
//            {
//                health.ApplyDamage(30f, WeaponType.Poison);
//                hunger = Mathf.Max(0, hunger - 25f);
//            }
//        }
//    }

    
//    public void Recargar()
//    {
//        bioEnergy += 15f * Time.deltaTime;
//        bioEnergy = Mathf.Clamp(bioEnergy, 0, maxBioEnergy);
//    }

//    public override void PerformAction() { }
//}