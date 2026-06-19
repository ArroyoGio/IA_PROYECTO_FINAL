//using UnityEngine;

//public abstract class HumanVehicleLand : AICharacterVehicle
//{
//    [Header("Human Vehicle Settings")]
//    public float rotationSpeed = 5f;
//    public float swimSpeed = 3f;

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

//    public void SwimToSurface(Vector3 surfacePoint)
//    {
//        Move(surfacePoint);
//    }

//    public override void UpdateAI()
//    {
//        // Las clases hijas implementan su l�gica espec�fica
//    }
//}