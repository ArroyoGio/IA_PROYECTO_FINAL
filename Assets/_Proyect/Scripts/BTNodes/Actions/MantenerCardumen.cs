using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MantenerCardumen : Action
{
    private FishActionLand fish;
    private OffsetPursuit offsetPursuit;
    private SteeringManager steering;

    public override void OnStart()
    {
        fish = GetComponent<FishActionLand>();
        offsetPursuit = GetComponent<OffsetPursuit>();
        steering = GetComponent<SteeringManager>();
    }

    public override TaskStatus OnUpdate()
    {
        if (fish == null || steering == null) return TaskStatus.Failure;

        // --- PRIMERO: Buscar líder si no tiene ---
        if (fish.schoolLeader == null)
        {
            GameObject[] allFish = GameObject.FindGameObjectsWithTag("Fish");
            foreach (var f in allFish)
            {
                if (f != gameObject)
                {
                    fish.schoolLeader = f.transform;
                    Debug.Log($" {gameObject.name} LÍDER ASIGNADO: {fish.schoolLeader.name}");
                    break;
                }
            }
        }

        // --- Si SIGUE sin líder, fallar ---
        if (fish.schoolLeader == null)
        {
            Debug.LogWarning($"{gameObject.name} no tiene líder y no encontró ningún Fish");
            return TaskStatus.Failure;
        }

        // --- FUERZA BRUTA: Configurar OffsetPursuit DIRECTAMENTE ---
        if (offsetPursuit != null)
        {
            // 1. Asignar líder
            offsetPursuit.SetLeader(fish.schoolLeader);

            // 2. Offset aleatorio
            Vector3 randomOffset = new Vector3(
                Random.Range(-fish.schoolDistance, fish.schoolDistance),
                0,
                Random.Range(-fish.schoolDistance, fish.schoolDistance)
            );
            offsetPursuit.SetOffset(randomOffset);

            // 3. Activar
            offsetPursuit.isActive = true;

            // 4. Agregar al SteeringManager
            steering.AddBehavior(offsetPursuit);

            // 5. DESACTIVAR WANDER (para que no compita)
            Wander wander = GetComponent<Wander>();
            if (wander != null)
            {
                wander.isActive = false;
                steering.RemoveBehavior(wander);
            }

            Debug.Log($"{gameObject.name}  OFFSET PURSUIT ACTIVADO. Siguiendo a {fish.schoolLeader.name}");
            return TaskStatus.Running;
        }
        else
        {
            Debug.LogError($"{gameObject.name} no tiene componente OffsetPursuit");
            return TaskStatus.Failure;
        }
    }
}