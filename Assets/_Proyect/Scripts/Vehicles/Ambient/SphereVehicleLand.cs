//using UnityEngine;

//public class SphereVehicleLand : AmbientVehicleLand
//{
//    [Header("Sphere Vehicle")]
//    public float bobSpeed = 0.5f;
//    public float bobHeight = 0.3f;
//    private Vector3 startPosition;

//    protected override void Awake()
//    {
//        base.Awake();
//        startPosition = transform.position;
//        isStationary = true;
//    }

//    void Update()
//    {
//        // Movimiento de flotación (bob)
//        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
//        transform.position = startPosition + Vector3.up * bobOffset;
//    }

//    public override void UpdateAI()
//    {
//        // La esfera solo flota, no necesita IA de movimiento
//    }
//}