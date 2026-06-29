using UnityEngine;

public class KrakenActionLand : PredatorActionLand
{
    [Header("Kraken")]
    public float TemporizadorDespertar = 0f;
    public float Ira = 0f;
    public float Fase = 0f;
    public float DistanciaPresa = 999f;

    private const float TiempoDespertar = 10f;
    private const float IraAtaque = 100f;
    private KrakenVehicleLand krakenVehicle;

    private void Awake()
    {
        LoadComponent();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        krakenVehicle = GetComponent<KrakenVehicleLand>();
    }

    private void Update()
    {
        UpdateAI();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();
        UpdateKrakenState();
        UpdateKrakenBlackboard();
    }

    public void Dormir()
    {
        Fase = 0f;
        Ira = 0f;

        if (krakenVehicle != null)
            krakenVehicle.Idle();
    }

    public void Despertar()
    {
        Fase = 1f;
        Ira = IraAtaque;
        TemporizadorDespertar = 0f;
    }

    public void AtraparPresa()
    {
        if (eye == null || eye.ViewEnemy == null)
            return;

        Transform prey = eye.ViewEnemy.transform;

        if (krakenVehicle != null)
            krakenVehicle.Acercarse();

        if (!IsTargetInRange(prey))
            return;

        Attack(prey);
        Ira = Mathf.Clamp(Ira - 20f, 0f, 100f);
    }

    public void VolverADormir()
    {
        if (krakenVehicle != null)
            krakenVehicle.Volver();

        Ira = Mathf.Clamp(Ira - 15f * Time.deltaTime, 0f, 100f);

        if (Ira <= 0f)
            Dormir();
    }

    private void UpdateKrakenState()
    {
        if (Fase <= 0f)
            TemporizadorDespertar += Time.deltaTime;

        if (eye != null && eye.ViewEnemy != null)
            DistanciaPresa = Vector3.Distance(transform.position, eye.ViewEnemy.transform.position);
        else
            DistanciaPresa = 999f;
    }

    private void UpdateKrakenBlackboard()
    {
        if (blackboard == null)
            return;

        blackboard.SetFloat("TemporizadorDespertar", TemporizadorDespertar);
        blackboard.SetFloat("Ira", Ira);
        blackboard.SetFloat("Fase", Fase);
        blackboard.SetFloat("DistanciaPresa", DistanciaPresa);
        blackboard.SetBool("Dormido", Fase <= 0f);
        blackboard.SetBool("PuedeDespertar", Fase <= 0f && TemporizadorDespertar >= TiempoDespertar);
        blackboard.SetBool("HayPresa", eye != null && eye.ViewEnemy != null);
    }
}
