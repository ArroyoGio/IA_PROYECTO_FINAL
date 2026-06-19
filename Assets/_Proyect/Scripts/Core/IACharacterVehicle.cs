using UnityEngine;
using UnityEngine.AI;

public abstract class IACharacterVehicle : AICharacterControl
{
    [Header("Vehicle Base Settings")]
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;

    public override void LoadComponent()
    {
        base.LoadComponent();
    }
    // falta definir e implementar las funciones de steering behaviors para el vehículo, como Seek, Flee, Arrive, Evade,etc.
    // tambien cincluye logica difuzza para usar en alguna funcion
    protected bool IsValidNavMeshPosition(Vector3 position)
    {
        return NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
    }
}