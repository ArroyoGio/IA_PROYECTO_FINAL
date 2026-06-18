using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HideNode : Action
{
    public SharedTransform threat;

    public override TaskStatus OnUpdate()
    {
        // Buscar el Steering Behavior "Hide" en el GameObject
        Hide hide = GetComponent<Hide>();

        if (hide == null)
        {
            Debug.LogWarning("No se encontró el componente Hide en " + gameObject.name);
            return TaskStatus.Failure;
        }

        // Configurar la amenaza
        if (threat.Value != null)
        {
            hide.SetThreat(threat.Value);
        }
        else
        {
            // Si no hay amenaza específica, usar la del AIEye
            AIEyeBase eye = GetComponent<AIEyeBase>();
            if (eye != null && eye.HasTargets())
            {
                hide.SetThreat(eye.GetNearestTarget());
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        // Activar el Hide
        hide.isActive = true;

        // Asegurarse de que el SteeringManager lo tenga
        SteeringManager steering = GetComponent<SteeringManager>();
        if (steering != null)
        {
            steering.AddBehavior(hide);
        }

        return TaskStatus.Success;
    }
}