using UnityEngine;

public class DolphinVehicleLand : PreyVehicleLand
{
    [Header("Dolphin Vehicle")]
    public float jumpHeight = 3f;
    public float jumpCooldown = 2f;
    private float lastJumpTime = 0f;

    public override void UpdateAI()
    {
        // El movimiento se maneja a través de SteeringManager
    }

    public void PerformJump()
    {
        if (Time.time - lastJumpTime > jumpCooldown && rb != null)
        {
            Vector3 jumpForce = Vector3.up * jumpHeight + transform.forward * 2f;
            rb.AddForce(jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }
    }

    public void MoveToSurface(Vector3 surfacePoint)
    {
        Move(surfacePoint);
    }

    public void SwimAroundSphere(Transform sphere)
    {
        if (sphere != null)
        {
            Vector3 orbitPosition = sphere.position + Random.insideUnitSphere * 5f;
            orbitPosition.y = 0;
            Move(orbitPosition);
        }
    }
}