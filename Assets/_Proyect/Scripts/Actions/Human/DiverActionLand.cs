using UnityEngine;

//public class DiverActionLand : HumanActionLand
//{
//    [Header("Diver Settings")]
//    public float oxygen = 100f;
//    public float maxOxygen = 100f;
//    public int ammo = 10;
//    public int maxAmmo = 10;
//    public float oxygenConsumption = 2f;
//    public float energyConsumption = 1.5f;
//    public float shootRange = 10f;

//    private Wander wander;
//    private Pursuit pursuit;
//    private Arrive arrive;

//    protected override void Awake()
//    {
//        base.Awake();
//        wander = GetComponent<Wander>();
//        pursuit = GetComponent<Pursuit>();
//        arrive = GetComponent<Arrive>();
//    }

//    // TODA la lógica va AQUÍ
//    public override void UpdateAI()
//    {
//        oxygen -= oxygenConsumption * Time.deltaTime;
//        energy -= energyConsumption * Time.deltaTime;
//        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
//        energy = Mathf.Clamp(energy, 0, maxEnergy);

//        if (blackboard != null)
//        {
//            blackboard.SetFloat("Oxygen", oxygen);
//            blackboard.SetFloat("Energy", energy);
//            blackboard.SetInt("Ammo", ammo);
//            blackboard.SetBool("OxigenoBajo", oxygen < 30f);
//            blackboard.SetBool("EnergiaBaja", energy < 30f);
//            blackboard.SetBool("MunicionBaja", ammo < 2);
//            blackboard.SetBool("TieneMunicion", ammo > 0);
//            blackboard.SetBool("VeCriatura", eye.HasTargets());
//            blackboard.SetBool("CriaturaAlcance", IsCriaturaInRange());
//        }
//    }

//    bool IsCriaturaInRange()
//    {
//        Transform target = eye.GetNearestTarget();
//        if (target != null)
//            return Vector3.Distance(transform.position, target.position) < shootRange;
//        return false;
//    }

//    public void GoToSurface()
//    {
//        Vector3 surfacePosition = new Vector3(transform.position.x, 0f, transform.position.z);
//        if (arrive != null)
//        {
//            GameObject surfaceTarget = new GameObject("SurfaceTarget");
//            surfaceTarget.transform.position = surfacePosition;
//            arrive.SetTarget(surfaceTarget.transform);
//            arrive.isActive = true;
//            steering.AddBehavior(arrive);
//        }
//        oxygen += 30f * Time.deltaTime;
//        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
//    }

//    public void RecargarMunicion() => ammo = maxAmmo;

//    public void Cazar()
//    {
//        Transform prey = eye.GetNearestTarget();
//        if (prey != null && ammo > 0)
//            Pursue(prey);
//    }

//    public void Disparar()
//    {
//        Transform prey = eye.GetNearestTarget();
//        if (prey != null && ammo > 0 && Vector3.Distance(transform.position, prey.position) < shootRange)
//        {
//            HealthBase health = prey.GetComponent<HealthBase>();
//            if (health != null)
//            {
//                health.ApplyDamage(35f, WeaponType.Normal);
//                ammo--;
//            }
//        }
//    }

//    public void Pursue(Transform target)
//    {
//        if (pursuit != null && target != null)
//        {
//            pursuit.SetTarget(target);
//            pursuit.isActive = true;
//            steering.AddBehavior(pursuit);
//        }
//    }

//    public void Explorar()
//    {
//        if (wander != null)
//        {
//            wander.isActive = true;
//            steering.AddBehavior(wander);
//        }
//    }

//    public override void PerformAction() { }
//}