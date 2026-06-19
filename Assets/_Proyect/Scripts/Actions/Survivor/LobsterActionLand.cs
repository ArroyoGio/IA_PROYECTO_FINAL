using UnityEngine;

//public class LobsterActionLand : SurvivorActionLand
//{
//    [Header("Lobster Settings")]
//    public float hunger = 50f;
//    public float maxHunger = 100f;
//    public float patience = 100f;
//    public float maxPatience = 100f;
//    public float burrowTime = 0f;
//    private bool isBurrowed = false;

//    private Wander wander;
//    private Hide hide;
//    private Arrive arrive;

//    protected override void Awake()
//    {
//        base.Awake();
//        wander = GetComponent<Wander>();
//        hide = GetComponent<Hide>();
//        arrive = GetComponent<Arrive>();
//    }

//    //TODA la lógica va AQUÍ
//    public override void UpdateAI()
//    {
//        hunger += 0.8f * Time.deltaTime;
//        hunger = Mathf.Clamp(hunger, 0, maxHunger);

//        if (!isBurrowed)
//        {
//            patience += 2f * Time.deltaTime;
//            patience = Mathf.Clamp(patience, 0, maxPatience);
//        }

//        if (blackboard != null)
//        {
//            blackboard.SetFloat("Hunger", hunger);
//            blackboard.SetFloat("Patience", patience);
//            blackboard.SetFloat("Camouflage", camouflage);
//            blackboard.SetBool("HambreAlta", hunger > 70f);
//            blackboard.SetBool("PacienciaAlta", patience > 50f);
//            blackboard.SetBool("PacienciaBaja", patience < 30f);
//            blackboard.SetBool("VeDepredador", eye.HasTargets());
//            blackboard.SetBool("VePresa", IsPreyNear());
//        }
//    }

//    bool IsPreyNear()
//    {
//        Collider[] prey = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Fish"));
//        return prey.Length > 0;
//    }

//    public void Enterrarse()
//    {
//        isBurrowed = true;
//        transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
//        if (hide != null)
//        {
//            hide.isActive = true;
//            steering.AddBehavior(hide);
//        }
//    }

//    public void Salir()
//    {
//        isBurrowed = false;
//        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
//        if (hide != null)
//            hide.isActive = false;
//    }

//    public void Emboscar()
//    {
//        if (!isBurrowed) Enterrarse();
//        patience -= Time.deltaTime * 5f;

//        Collider[] prey = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Fish"));
//        if (prey.Length > 0)
//        {
//            Transform target = prey[0].transform;
//            if (arrive != null)
//            {
//                arrive.SetTarget(target);
//                arrive.isActive = true;
//                steering.AddBehavior(arrive);
//            }
//        }
//    }

//    public void Attack()
//    {
//        Collider[] prey = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Fish"));
//        foreach (var p in prey)
//        {
//            HealthBase health = p.GetComponent<HealthBase>();
//            if (health != null)
//            {
//                health.ApplyDamage(20f, WeaponType.Normal);
//                hunger = Mathf.Max(0, hunger - 20f);
//                patience = Mathf.Min(maxPatience, patience + 10f);
//            }
//        }
//    }

//    public void CambiarPosicion()
//    {
//        if (isBurrowed) Salir();
//        Vector3 newPosition = transform.position + Random.insideUnitSphere * 10f;
//        newPosition.y = 0;
//        steering.SetTarget(newPosition);
//        patience += 20f;
//        patience = Mathf.Clamp(patience, 0, maxPatience);
//    }

//    public override void PerformAction() { }
//}