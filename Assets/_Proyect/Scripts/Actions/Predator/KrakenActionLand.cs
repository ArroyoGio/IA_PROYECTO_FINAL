//using UnityEngine;


//public class KrakenActionLand : PredatorActionLand
//{
//    [Header("Kraken Settings")]
//    public float wakeTimer = 0f;
//    public float wakeTime = 30f;
 
//    public int[] phaseThresholds = { 70, 40 };

//    public LayerMask preyLayer;
//    void Awake()
//    {
//        LoadComponent();
//        //wander = GetComponent<Wander>();
//        //pursuit = GetComponent<Pursuit>();
//    }
//    public override void LoadComponent()
//    {
//        base.LoadComponent();

//    }
//    private void Update()
//    {
//        UpdateAI();

//    }
//    //TODA la lógica va AQUÍ
//    public override void UpdateAI()
//    {
//        wakeTimer += Time.deltaTime;
//        base.UpdateAI();
//    }

//    public void Despertar()
//    {
//        wakeTimer = 0f;
//    }

//    public int SeleccionarFase()
//    {
//        //float healthPercent = (health / maxHealth) * 100f;

//        //if (healthPercent > phaseThresholds[0])
//        //    phase = 1;
//        //else if (healthPercent > phaseThresholds[1])
//        //    phase = 2;
//        //else
//        //    phase = 3;

//        //return phase;
//        return 0;
//    }

//    public void AtacarConTentaculo()
//    {
//        Transform prey = eye.GetNearestTarget();
//        if (prey != null && Vector3.Distance(transform.position, prey.position) < attackRange * 2f)
//        {
//            HealthBase health = prey.GetComponent<HealthBase>();
//            if (health != null)
//                health.ApplyDamage(50f, WeaponType.Tentacle);
//        }
//    }

//    public void CrearTorbellino()
//    {
//        Collider[] targets = Physics.OverlapSphere(transform.position, 15f, preyLayer);
//        foreach (var target in targets)
//        {
//            HealthBase health = target.GetComponent<HealthBase>();
//            if (health != null)
//                health.ApplyDamage(30f, WeaponType.Whirlpool);
//        }
//    }

//    public void RayoElectrico()
//    {
//        Transform prey = eye.GetNearestTarget();
//        if (prey != null)
//        {
//            HealthBase health = prey.GetComponent<HealthBase>();
//            if (health != null)
//                health.ApplyDamage(40f, WeaponType.Lightning);
//        }
//    }

//    public void Dormir()
//    {
//        wakeTimer = 0f;
//    }

//    public void IncrementarTemporizador()
//    {
//        wakeTimer += Time.deltaTime;
//    }

//    public override void PerformAction() { }
//}