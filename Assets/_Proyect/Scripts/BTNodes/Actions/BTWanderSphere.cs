using BehaviorDesigner.Runtime.Tasks;

public class BTWanderSphere : Action
{
    private SphereVehicleLand sphereVehicle;

    public override void OnAwake()
    {
        sphereVehicle = GetComponent<SphereVehicleLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (sphereVehicle == null)
            return TaskStatus.Failure;

        sphereVehicle.PatrullarSuave();
        return TaskStatus.Success;
    }
}
