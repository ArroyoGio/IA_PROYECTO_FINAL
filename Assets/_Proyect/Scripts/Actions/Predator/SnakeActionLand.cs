using UnityEngine;

public class SnakeActionLand : PredatorActionLand
{
    [Header("Snake Settings")]
    public float bioluminescentEnergy = 100f;
    public float stealth = 70f;
    public bool preyConfused = false;
    public float flashCost = 30f;
    public float flashRange = 20f;
    public float attackCooldown = 2f;
    public ParticleSystem flashParticlePrefab;
    public float flashEffectDuration = 2f;

    private SnakeVehicleLand snakeVehicle;
    private float lastAttackTime = -999f;
    private float distanciaPresa;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        snakeVehicle = GetComponent<SnakeVehicleLand>();
    }

    private void Update()
    {
        UpdateAI();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateSnakeState();
        UpdateSnakeBlackboard();
    }

    public void MovimientoSigiloso()
    {
        if (snakeVehicle != null)
            snakeVehicle.MoveStealth();
    }

    public void EmitirDestello()
    {
        if (!CanFlash())
            return;

        bioluminescentEnergy = Mathf.Clamp(bioluminescentEnergy - flashCost, 0f, 100f);
        preyConfused = true;
        CreateFlashVisual();
    }

    public void Attack()
    {
        if (!preyConfused)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (eye == null || eye.ViewEnemy == null)
            return;

        Transform prey = eye.ViewEnemy.transform;
        if (!IsTargetInRange(prey))
            return;

        HealthBase preyHealth = prey.GetComponentInParent<HealthBase>();
        if (preyHealth == null || preyHealth.IsDead)
            return;

        preyHealth.ApplyDamage(attackDamage, weaponType);
        lastAttackTime = Time.time;
        hunger = Mathf.Clamp(hunger - 25f, 0f, maxHunger);
        preyConfused = false;
    }

    public void RecargarBioluminiscencia()
    {
        bioluminescentEnergy = Mathf.Clamp(bioluminescentEnergy + 15f * Time.deltaTime, 0f, 100f);
    }

    public bool CanFlash()
    {
        return eye != null &&
               eye.ViewEnemy != null &&
               bioluminescentEnergy >= flashCost &&
               Vector3.Distance(transform.position, eye.ViewEnemy.transform.position) <= flashRange;
    }

    private void UpdateSnakeState()
    {
        if (eye != null && eye.ViewEnemy != null)
            distanciaPresa = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        else
            distanciaPresa = float.MaxValue;

        RecargarBioluminiscencia();
    }

    private void UpdateSnakeBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("BioluminescentEnergy", bioluminescentEnergy);
        blackboard.SetFloat("Stealth", stealth);
        blackboard.SetFloat("DistanciaPresa", distanciaPresa);
        blackboard.SetBool("PreyConfused", preyConfused);
        blackboard.SetBool("PuedeEmitirDestello", CanFlash());
    }

    private void CreateFlashVisual()
    {
        if (flashParticlePrefab == null)
            return;

        ParticleSystem flashEffect = Instantiate(flashParticlePrefab, transform.position, Quaternion.identity);
        flashEffect.Play();
        Destroy(flashEffect.gameObject, flashEffectDuration);
    }
}
