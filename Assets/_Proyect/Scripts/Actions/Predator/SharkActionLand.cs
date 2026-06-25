using UnityEngine;

public class SharkActionLand : PredatorActionLand
{
    [Header("Shark Attack")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        LoadComponent();
    }

    private void Update()
    {
        UpdateAI();

        if (eye != null && eye.ViewEnemy != null)
        {
            Morder();
        }
    }

    public void Morder()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        if (eye == null || eye.ViewEnemy == null) return;

        Transform prey = eye.ViewEnemy.transform;

        Attack(prey);

        lastAttackTime = Time.time;

        hunger = Mathf.Max(0, hunger - 30f);
        energy = Mathf.Max(0, energy - 5f);
        aggressiveness = Mathf.Min(100, aggressiveness + 5f);
    }

    public void Descansar()
    {
        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }
}