using UnityEngine;

public class SharkActionLand : PredatorActionLand
{
   
     void Awake()
    {
        LoadComponent();
     
    }
    public override void LoadComponent()
    {
        base.LoadComponent();

    }
    private void Update()
    {
        base.UpdateAI();

    }
    // TODA la lógica va AQUÍ
     
    public void Morder()
    {
        Transform prey = eye.ViewEnemy.transform;
        Attack(prey);
        if (prey != null)
        {
            hunger = Mathf.Max(0, hunger - 30f);
            energy = Mathf.Max(0, energy - 5f);
            aggressiveness = Mathf.Min(100, aggressiveness + 5f);
        }
    }

     
    public void Descansar()
    {
        energy += 10f * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        
    }

    
}