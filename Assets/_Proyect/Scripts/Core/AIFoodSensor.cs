using UnityEngine;

public class AIFoodSensor : MonoBehaviour
{
    public float detectionRadius = 15f;
    public LayerMask foodLayer;

    private readonly Collider[] foods = new Collider[32];
    private AlienFoodItem nearestFood;

    public AlienFoodItem NearestFood => nearestFood;
    public Transform FoodTarget => nearestFood != null ? nearestFood.transform : null;
    public bool HasFood => nearestFood != null;

    private void Update()
    {
        ScanFood();
    }

    public void ScanFood()
    {
        nearestFood = null;

        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRadius,
            foods,
            foodLayer);

        float nearestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider foodCollider = foods[i];
            if (foodCollider == null)
                continue;

            AlienFoodItem food = foodCollider.GetComponentInParent<AlienFoodItem>();
            if (food == null)
                continue;

            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestFood = food;
            }
        }
    }
}
