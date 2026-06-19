//using UnityEngine;

//public abstract class SurvivorVehicleLand : AICharacterVehicle
//{
//    [Header("Survivor Vehicle Settings")]
//    public float rotationSpeed = 3f;
//    public float hideSpeed = 1f;

//    public override void Move(Vector3 target)
//    {
//        if (rb == null) return;

//        Vector3 direction = (target - transform.position).normalized;
//        Vector3 desiredVelocity = direction * maxSpeed;
//        Vector3 steeringForce = desiredVelocity - rb.linearVelocity;
//        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

//        rb.AddForce(steeringForce, ForceMode.Force);

//        if (rb.linearVelocity.magnitude > 0.1f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
//        }
//    }

//    public void MoveToHide(Vector3 hideSpot)
//    {
//        float originalSpeed = maxSpeed;
//        maxSpeed = hideSpeed;
//        Move(hideSpot);
//        maxSpeed = originalSpeed;
//    }

//    public override void UpdateAI()
//    {
//        // Las clases hijas implementan su l�gica espec�fica
//    }
//}