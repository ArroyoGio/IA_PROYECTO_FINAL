using UnityEngine;

//public class SnakeVehicleLand : PredatorVehicleLand
//{
//    [Header("Snake Vehicle")]
//    public float stealthSpeed = 1.5f;
//    public float attackSpeed = 4f;
//    public float constrictRange = 2.5f;

//    public override void UpdateAI()
//    {
//        // El movimiento se maneja a través de SteeringManager
//    }

//    public void MoveStealthily(Vector3 target)
//    {
//        float originalSpeed = maxSpeed;
//        maxSpeed = stealthSpeed;
//        Move(target);
//        maxSpeed = originalSpeed;
//    }

//    public void AttackDash(Transform prey)
//    {
//        if (prey != null)
//        {
//            float originalSpeed = maxSpeed;
//            maxSpeed = attackSpeed;
//            Move(prey.position);
//            maxSpeed = originalSpeed;
//        }
//    }

//    public bool IsInConstrictRange(Transform target)
//    {
//        if (target == null) return false;
//        return Vector3.Distance(transform.position, target.position) < constrictRange;
//    }
//}