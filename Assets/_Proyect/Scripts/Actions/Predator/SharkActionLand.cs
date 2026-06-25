using UnityEngine;

public class SharkActionLand : PredatorActionLand
{
    private void Awake()
    {
        LoadComponent();
    }

    private void Update()
    {
        UpdateAI();
    }

    public void Morder()
    {
        if (eye == null || eye.ViewEnemy == null) return;

        Transform prey = eye.ViewEnemy.transform;

        Attack(prey);

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