using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ─────────────────────────────────────────────────────────────────────────────
// Datos de un hueso de la cadena IK
// ─────────────────────────────────────────────────────────────────────────────
[System.Serializable]
public class FishBoneData
{
    public Transform bone;

    [Tooltip("Ángulo máximo que este hueso puede rotar respecto a su posición inicial.")]
    [Range(0f, 90f)]
    public float maxRotation = 30f;

    [Tooltip("Cuánto contribuye este hueso a la rotación total (0 = nada, 1 = total).")]
    [Range(0f, 1f)]
    public float influence = 1f;

    [HideInInspector] public Quaternion initialLocalRotation;
    [HideInInspector] public Quaternion smoothedRotation;
}

// ─────────────────────────────────────────────────────────────────────────────
// Controlador principal
// ─────────────────────────────────────────────────────────────────────────────
public class FishIKControllerClaude : MonoBehaviour
{
    // ── Target ────────────────────────────────────────────────────────────────
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool lookAtTarget = true;

    // ── Auto-detección ────────────────────────────────────────────────────────
    [Header("Auto-detección de Huesos")]
    [Tooltip("Transform raíz desde donde buscar la jerarquía. Si es null se usa este GameObject.")]
    [SerializeField] private Transform skeletonRoot;

    [Tooltip("Palabras clave (case-insensitive) para detectar huesos de columna automáticamente.")]
    [SerializeField] private string[] spineKeywords = { "spine", "body", "torso", "vertebra" };

    [Tooltip("Palabras clave para la cabeza.")]
    [SerializeField] private string[] headKeywords = { "head", "cabeza" };

    [Tooltip("Palabras clave para la cola.")]
    [SerializeField] private string[] tailKeywords = { "tail", "cola", "fin" };

    // ── Huesos (configurables también a mano) ────────────────────────────────
    [Header("Huesos")]
    [SerializeField] private Transform headBone;
    [SerializeField] private List<FishBoneData> spineBones = new List<FishBoneData>();
    [SerializeField] private Transform tailBone;

    // ── Velocidad ─────────────────────────────────────────────────────────────
    [Header("Velocidad")]
    [Tooltip("Qué tan rápido el cuerpo rota para mirar al target.")]
    [SerializeField] private float bodyRotationSpeed = 5f;

    [Tooltip("Suavizado de los huesos. Valores bajos = más suave pero más lento.")]
    [Range(0.01f, 1f)]
    [SerializeField] private float boneSmoothFactor = 0.12f;

    // ── Límites ───────────────────────────────────────────────────────────────
    [Header("Límites de Rotación")]
    [SerializeField] private float maxHeadRotationY = 45f;
    [SerializeField] private float maxHeadRotationZ = 15f;
    [SerializeField] private float maxSpineRotationY = 30f;
    [SerializeField] private float maxSpineRotationZ = 10f;

    // ── Cola ──────────────────────────────────────────────────────────────────
    [Header("Cola")]
    [SerializeField] private float tailWagSpeed = 2.5f;
    [SerializeField] private float tailWagAmount = 18f;

    // ── Gizmos ────────────────────────────────────────────────────────────────
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private float gizmoBoneSize = 0.05f;
    [SerializeField] private bool showBoneLabels = false;
    [SerializeField] private Color spineGizmoColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color headGizmoColor = Color.red;
    [SerializeField] private Color tailGizmoColor = Color.blue;
    [SerializeField] private Color targetGizmoColor = Color.green;

    // ── Estado interno ────────────────────────────────────────────────────────
    private Quaternion _initialHeadRotation;
    private Quaternion _smoothedHeadRotation;
    private Quaternion _initialTailRotation;
    private float _tailTimer;
    private bool _initialized;

    // =========================================================================
    // Unity lifecycle
    // =========================================================================

    private void Start()
    {
        if (skeletonRoot == null) skeletonRoot = transform;
        Initialize();
    }

    private void LateUpdate()
    {
        if (!_initialized || !lookAtTarget || target == null) return;

        _tailTimer += Time.deltaTime * tailWagSpeed;
        ApplyBodyRotation();
        ApplySpineIK();
        ApplyHeadIK();
        ApplyTailWag();
    }

    // =========================================================================
    // Inicialización
    // =========================================================================

    private void Initialize()
    {
        // Cabeza
        if (headBone != null)
        {
            _initialHeadRotation = headBone.localRotation;
            _smoothedHeadRotation = _initialHeadRotation;
        }

        // Columna
        foreach (var bd in spineBones)
        {
            if (bd.bone == null) continue;
            bd.initialLocalRotation = bd.bone.localRotation;
            bd.smoothedRotation = bd.initialLocalRotation;
        }

        // Cola
        if (tailBone != null)
            _initialTailRotation = tailBone.localRotation;

        _initialized = true;
        Debug.Log($"[FishIK] Inicializado — cabeza: {(headBone ? headBone.name : "ninguna")}, " +
                  $"spine bones: {spineBones.Count}, cola: {(tailBone ? tailBone.name : "ninguna")}");
    }

    // =========================================================================
    // IK logic
    // =========================================================================

    private void ApplyBodyRotation()
    {
        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                                                 Time.deltaTime * bodyRotationSpeed);
    }

    private void ApplySpineIK()
    {
        if (spineBones.Count == 0) return;

        Vector3 localDir = transform.InverseTransformDirection(
            (target.position - transform.position).normalized);

        // Cada hueso recibe una fracción de la rotación total,
        // ponderada por su influence y atenuada por su posición en la cadena.
        for (int i = 0; i < spineBones.Count; i++)
        {
            FishBoneData bd = spineBones[i];
            if (bd.bone == null) continue;

            // El hueso más cercano a la cabeza recibe más rotación
            float chainFactor = 1f - (float)i / spineBones.Count;
            float effectiveMax = maxSpineRotationY * bd.influence * chainFactor;

            Quaternion rawRot = Quaternion.FromToRotation(Vector3.forward, localDir);
            Vector3 euler = ClampEuler(rawRot.eulerAngles,
                                             effectiveMax,
                                             maxSpineRotationZ * bd.influence * chainFactor);

            bd.smoothedRotation = Quaternion.Slerp(
                bd.smoothedRotation,
                Quaternion.Euler(euler),
                boneSmoothFactor);

            bd.bone.localRotation = bd.initialLocalRotation * bd.smoothedRotation;
        }
    }

    private void ApplyHeadIK()
    {
        if (headBone == null) return;

        Vector3 localDir = headBone.InverseTransformDirection(
            (target.position - headBone.position).normalized);

        Quaternion rawRot = Quaternion.FromToRotation(Vector3.forward, localDir);
        Vector3 euler = ClampEuler(rawRot.eulerAngles, maxHeadRotationY, maxHeadRotationZ);

        _smoothedHeadRotation = Quaternion.Slerp(
            _smoothedHeadRotation,
            Quaternion.Euler(euler),
            boneSmoothFactor * 1.5f);   // la cabeza responde un poco más rápido

        headBone.localRotation = _initialHeadRotation * _smoothedHeadRotation;
    }

    private void ApplyTailWag()
    {
        if (tailBone == null) return;
        float angle = Mathf.Sin(_tailTimer) * tailWagAmount;
        tailBone.localRotation = _initialTailRotation * Quaternion.Euler(0f, 0f, angle);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// Convierte eulerAngles (0-360) a rango (-180, 180) y aplica clamp.
    private static Vector3 ClampEuler(Vector3 euler, float maxY, float maxZ)
    {
        float x = WrapAngle(euler.x);
        float y = WrapAngle(euler.y);
        float z = WrapAngle(euler.z);

        return new Vector3(
            Mathf.Clamp(x, -maxY * 0.3f, maxY * 0.3f),
            Mathf.Clamp(y, -maxY, maxY),
            Mathf.Clamp(z, -maxZ, maxZ));
    }

    private static float WrapAngle(float angle)
        => angle > 180f ? angle - 360f : angle;

    // =========================================================================
    // Auto-detección de huesos  (llamar desde el Editor o en Start)
    // =========================================================================

    /// Busca en toda la jerarquía de skeletonRoot y rellena los campos
    /// headBone, spineBones y tailBone según las palabras clave configuradas.
    public void AutoDetectBones()
    {
        if (skeletonRoot == null) skeletonRoot = transform;

        Transform[] allChildren = skeletonRoot.GetComponentsInChildren<Transform>(includeInactive: true);

        headBone = null;
        tailBone = null;
        spineBones.Clear();

        // Lista temporal con índice de orden en jerarquía para ordenar la columna
        var spineFound = new List<(Transform t, int depth)>();

        foreach (Transform t in allChildren)
        {
            if (t == skeletonRoot) continue;

            string nameLower = t.name.ToLowerInvariant();

            if (headBone == null && MatchesAny(nameLower, headKeywords))
            {
                headBone = t;
                continue;
            }

            if (tailBone == null && MatchesAny(nameLower, tailKeywords))
            {
                tailBone = t;
                continue;
            }

            if (MatchesAny(nameLower, spineKeywords))
            {
                spineFound.Add((t, GetHierarchyDepth(t)));
            }
        }

        // Ordenar columna de raíz a punta (menor profundidad primero)
        spineFound.Sort((a, b) => a.depth.CompareTo(b.depth));

        foreach (var (t, _) in spineFound)
        {
            spineBones.Add(new FishBoneData
            {
                bone = t,
                maxRotation = maxSpineRotationY,
                influence = 1f
            });
        }

        Debug.Log($"[FishIK] Auto-detect — cabeza: {(headBone ? headBone.name : "—")}, " +
                  $"spine: {spineBones.Count}, cola: {(tailBone ? tailBone.name : "—")}");
    }

    private static bool MatchesAny(string nameLower, string[] keywords)
    {
        foreach (string kw in keywords)
            if (nameLower.Contains(kw.ToLowerInvariant())) return true;
        return false;
    }

    private static int GetHierarchyDepth(Transform t)
    {
        int depth = 0;
        while (t.parent != null) { t = t.parent; depth++; }
        return depth;
    }

    // =========================================================================
    // API pública
    // =========================================================================

    public void SetTarget(Transform newTarget) => target = newTarget;
    public void SetLookAt(bool enabled) => lookAtTarget = enabled;
    public Transform GetHeadBone() => headBone;
    public Transform GetTailBone() => tailBone;
    public List<FishBoneData> GetSpineBones() => spineBones;

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Transform root = skeletonRoot != null ? skeletonRoot : transform;
        DrawSkeletonGizmos(root, selected: false);
        DrawTargetGizmo(selected: false);
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Transform root = skeletonRoot != null ? skeletonRoot : transform;
        DrawSkeletonGizmos(root, selected: true);
        DrawTargetGizmo(selected: true);

        // Línea de influencia de cada spine bone
        foreach (var bd in spineBones)
        {
            if (bd.bone == null || target == null) continue;
            Gizmos.color = new Color(spineGizmoColor.r, spineGizmoColor.g, spineGizmoColor.b, 0.25f);
            Gizmos.DrawLine(bd.bone.position, target.position);
        }
    }

    private void DrawSkeletonGizmos(Transform root, bool selected)
    {
        float sizeMultiplier = selected ? 1.4f : 1f;

        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child == root || child.parent == null) continue;

            Color col = GetBoneGizmoColor(child);
            float size = gizmoBoneSize * sizeMultiplier;

            // Línea al padre
            Gizmos.color = new Color(col.r, col.g, col.b, selected ? 0.9f : 0.6f);
            Gizmos.DrawLine(child.position, child.parent.position);

            // Esfera en el hueso
            Gizmos.color = col;
            Gizmos.DrawSphere(child.position, size);
        }

        // Root
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(root.position, gizmoBoneSize * 1.6f * sizeMultiplier);
        Gizmos.DrawWireSphere(root.position, gizmoBoneSize * 2.4f * sizeMultiplier);

        // Labels en Editor
#if UNITY_EDITOR
        if (showBoneLabels && selected)
        {
            GUIStyle style = new GUIStyle
            {
                normal = { textColor = Color.white },
                fontSize = 9,
                alignment = TextAnchor.MiddleCenter
            };
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child == root) continue;
                Handles.Label(child.position + Vector3.up * gizmoBoneSize * 2.5f, child.name, style);
            }
        }
#endif
    }

    private void DrawTargetGizmo(bool selected)
    {
        if (target == null) return;

        float size = gizmoBoneSize * (selected ? 3.5f : 2.5f);

        Gizmos.color = targetGizmoColor;
        Gizmos.DrawSphere(target.position, size);
        Gizmos.DrawWireSphere(target.position, size * 1.5f);

        // Línea desde la cabeza (o root) al target
        Transform from = headBone != null ? headBone : (skeletonRoot != null ? skeletonRoot : transform);
        Gizmos.color = new Color(targetGizmoColor.r, targetGizmoColor.g, targetGizmoColor.b, 0.35f);
        Gizmos.DrawLine(from.position, target.position);
    }

    private Color GetBoneGizmoColor(Transform bone)
    {
        if (headBone != null && bone == headBone) return headGizmoColor;
        if (tailBone != null && bone == tailBone) return tailGizmoColor;
        foreach (var bd in spineBones)
            if (bd.bone != null && bone == bd.bone) return spineGizmoColor;
        return new Color(0.6f, 0.85f, 1f);
    }
}

