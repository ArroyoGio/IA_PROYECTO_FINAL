using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoneData
{
    public Transform bone;
    [Range(0, 90)] public float maxRotation = 30f;
    [Range(0, 1)] public float influence = 1f;
}

public class FishIKController : MonoBehaviour
{
    [Header("🎯 OBJETIVO")]
    [SerializeField] private Transform target;
    [SerializeField] private bool lookAtTarget = true;

    [Header("⚡ VELOCIDAD")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float smoothingFactor = 0.1f;

    [Header("🦴 HUESOS")]
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform headBone;
    [SerializeField] private List<BoneData> spineBones = new List<BoneData>();
    [SerializeField] private Transform tailBone;

    [Header("🔄 LÍMITES DE ROTACIÓN")]
    [SerializeField] private float maxHeadRotation = 45f;
    [SerializeField] private float maxSpineRotation = 30f;

    [Header("🐟 COLA")]
    [SerializeField] private float tailWagSpeed = 2f;
    [SerializeField] private float tailWagAmount = 15f;

    [Header("🎨 GIZMOS")]
    public bool showGizmos = true; // ⭐ NUEVO: Toggle para Gizmos
    [SerializeField] private float gizmoSphereSize = 0.05f;
    [SerializeField] private float rootSphereSize = 0.08f;
    [SerializeField] private bool showBoneLabels = false;

    // Variables internas
    private Quaternion initialHeadRotation;
    private Quaternion initialTailRotation;
    private Quaternion[] initialSpineRotations;
    private Quaternion smoothedHeadRotation;
    private Quaternion[] smoothedSpineRotations;
    private float tailTimer = 0f;
    private bool hasInitialized = false;
    void Start()
    {
        InitializeBones();
    }

    void Update()
    {
        if (!hasInitialized) return;

        tailTimer += Time.deltaTime * tailWagSpeed;

        if (lookAtTarget && target != null)
        {
            LookAtTarget();
        }

        AnimateTail();
    }

    void InitializeBones()
    {
        if (headBone != null)
        {
            initialHeadRotation = headBone.localRotation;
            smoothedHeadRotation = initialHeadRotation;
        }

        if (spineBones.Count > 0)
        {
            initialSpineRotations = new Quaternion[spineBones.Count];
            smoothedSpineRotations = new Quaternion[spineBones.Count];

            for (int i = 0; i < spineBones.Count; i++)
            {
                if (spineBones[i].bone != null)
                {
                    initialSpineRotations[i] = spineBones[i].bone.localRotation;
                    smoothedSpineRotations[i] = initialSpineRotations[i];
                }
            }
        }

        if (tailBone != null)
        {
            initialTailRotation = tailBone.localRotation;
        }

        hasInitialized = true;
        Debug.Log("✅ Huesos inicializados correctamente");
    }

    void LookAtTarget()
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0;

        if (directionToTarget.magnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // IK para columna vertebral
        if (spineBones.Count > 0)
        {
            for (int i = 0; i < spineBones.Count; i++)
            {
                if (spineBones[i].bone == null) continue;

                float influence = spineBones[i].influence;
                float currentMaxRotation = maxSpineRotation * influence;

                Quaternion localTargetRot = Quaternion.FromToRotation(
                    Vector3.forward,
                    transform.InverseTransformDirection(directionToTarget)
                );

                Vector3 localEuler = localTargetRot.eulerAngles;
                localEuler.x = Mathf.Clamp(localEuler.x, -currentMaxRotation, currentMaxRotation);
                localEuler.y = Mathf.Clamp(localEuler.y, -currentMaxRotation, currentMaxRotation);
                localEuler.z = Mathf.Clamp(localEuler.z, -currentMaxRotation * 0.5f, currentMaxRotation * 0.5f);

                Quaternion limitedRotation = Quaternion.Euler(localEuler);

                smoothedSpineRotations[i] = Quaternion.Slerp(
                    smoothedSpineRotations[i],
                    limitedRotation,
                    smoothingFactor
                );

                spineBones[i].bone.localRotation = initialSpineRotations[i] * smoothedSpineRotations[i];
            }
        }

        // IK para cabeza
        if (headBone != null)
        {
            Quaternion headTargetRot = Quaternion.FromToRotation(
                Vector3.forward,
                transform.InverseTransformDirection(directionToTarget)
            );

            Vector3 headEuler = headTargetRot.eulerAngles;
            headEuler.x = Mathf.Clamp(headEuler.x, -maxHeadRotation, maxHeadRotation);
            headEuler.y = Mathf.Clamp(headEuler.y, -maxHeadRotation, maxHeadRotation);
            headEuler.z = Mathf.Clamp(headEuler.z, -maxHeadRotation * 0.3f, maxHeadRotation * 0.3f);

            Quaternion limitedHeadRot = Quaternion.Euler(headEuler);

            smoothedHeadRotation = Quaternion.Slerp(
                smoothedHeadRotation,
                limitedHeadRot,
                smoothingFactor
            );

            headBone.localRotation = initialHeadRotation * smoothedHeadRotation;
        }
    }

    void AnimateTail()
    {
        if (tailBone == null) return;

        float wagAngle = Mathf.Sin(tailTimer) * tailWagAmount;
        tailBone.localRotation = initialTailRotation * Quaternion.Euler(0, 0, wagAngle);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Debug.Log($"🎯 Nuevo objetivo asignado: {newTarget.name}");
    }

    public void SetLookAt(bool enabled)
    {
        lookAtTarget = enabled;
    }

    // ============================================
    // 🎨 GIZMOS - DIBUJA TODOS LOS HUESOS
    // ============================================
    void OnDrawGizmos()
    {
        // ⭐ Si showGizmos está desactivado, no dibuja nada
        if (!showGizmos) return;
        if (rootBone == null) return;

        // Obtener todos los huesos hijos del root
        List<Transform> allBones = GetAllChildBones(rootBone);

        // Dibujar líneas entre cada hueso y su padre
        foreach (Transform bone in allBones)
        {
            if (bone.parent == null) continue;

            // Color según el tipo de hueso
            Color boneColor = GetBoneColor(bone);
            Gizmos.color = boneColor;

            // Dibujar línea desde el hueso a su padre
            Gizmos.DrawLine(bone.position, bone.parent.position);

            // Dibujar esfera en cada hueso
            Gizmos.DrawSphere(bone.position, gizmoSphereSize);
        }

        // Dibujar el Root con color especial
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rootBone.position, rootSphereSize);
        Gizmos.DrawWireSphere(rootBone.position, rootSphereSize * 1.5f);

        // Si hay objetivo, dibujar línea del root al objetivo
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rootBone.position, target.position);
            Gizmos.DrawSphere(target.position, gizmoSphereSize * 2f);
        }

        // Dibujar etiquetas con nombres si está activado
        if (showBoneLabels)
        {
            DrawBoneLabels(allBones);
        }
    }

    // ============================================
    // MÉTODOS AUXILIARES PARA GIZMOS
    // ============================================

    List<Transform> GetAllChildBones(Transform parent)
    {
        List<Transform> bones = new List<Transform>();

        foreach (Transform child in parent)
        {
            bones.Add(child);
            bones.AddRange(GetAllChildBones(child));
        }

        return bones;
    }

    Color GetBoneColor(Transform bone)
    {
        // Cabeza - Rojo
        if (headBone != null && bone == headBone)
            return Color.red;

        // Cola - Azul
        if (tailBone != null && bone == tailBone)
            return Color.blue;

        // Columna vertebral - Naranja
        foreach (var spineData in spineBones)
        {
            if (spineData.bone != null && bone == spineData.bone)
                return new Color(1f, 0.5f, 0f);
        }

        // Huesos normales - Cian claro
        return new Color(0.5f, 0.8f, 1f);
    }

    void DrawBoneLabels(List<Transform> bones)
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 10;
        style.alignment = TextAnchor.MiddleCenter;

        foreach (Transform bone in bones)
        {
            Vector3 labelPos = bone.position + Vector3.up * (gizmoSphereSize * 3f);
            UnityEditor.Handles.Label(labelPos, bone.name, style);
        }
    }

    // ============================================
    // GIZMOS EN MODO SELECTED (más detallado)
    // ============================================
    void OnDrawGizmosSelected()
    {
        // ⭐ Si showGizmos está desactivado, no dibuja nada
        if (!showGizmos) return;
        if (rootBone == null) return;

        // Dibujar jerarquía completa con colores más brillantes
        List<Transform> allBones = GetAllChildBones(rootBone);

        foreach (Transform bone in allBones)
        {
            if (bone.parent == null) continue;

            // Línea más gruesa en selección
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(bone.position, bone.parent.position);

            // Esfera más grande
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(bone.position, gizmoSphereSize * 1.5f);
        }

        // Marcar el Root
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rootBone.position, rootSphereSize * 1.5f);

        // Dibujar líneas de conexión entre huesos de la columna
        if (spineBones.Count > 1)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            for (int i = 0; i < spineBones.Count - 1; i++)
            {
                if (spineBones[i].bone != null && spineBones[i + 1].bone != null)
                {
                    Gizmos.DrawLine(
                        spineBones[i].bone.position,
                        spineBones[i + 1].bone.position
                    );
                }
            }
        }

        // Dibujar el objetivo con una esfera más grande
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target.position, gizmoSphereSize * 3f);

            // Línea discontinua al objetivo
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawLine(rootBone.position, target.position);
        }
    }
}