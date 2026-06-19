using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// =============================================================================
// Custom Editor — botón "Auto-Detectar Huesos" en el Inspector
// =============================================================================
#if UNITY_EDITOR
[CustomEditor(typeof(FishIKController))]
public class FishIKControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(8);

        FishIKControllerClaude ik = (FishIKControllerClaude)target;

        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.4f);
        if (GUILayout.Button("🔍  Auto-Detectar Huesos", GUILayout.Height(32)))
        {
            Undo.RecordObject(ik, "Auto-Detect Fish Bones");
            ik.AutoDetectBones();
            EditorUtility.SetDirty(ik);
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.HelpBox(
            "Busca en la jerarquía de 'Skeleton Root' usando las palabras clave configuradas.\n" +
            "Puedes editar los arrays de keywords para adaptarlos a tu naming convention.",
            MessageType.Info);
    }
}
#endif