using UnityEngine;

public static class DebugDraw
{
    // Dibujar un círculo en el editor
    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
#if UNITY_EDITOR
        Vector3 prevPoint = center + Vector3.forward * radius;

        for (int i = 0; i < segments; i++)
        {
            float angle = (i + 1) * 2f * Mathf.PI / segments;
            Vector3 newPoint = center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
#endif
    }

    // Dibujar un arco de visión
    public static void DrawVisionCone(Vector3 origin, Vector3 direction, float angle, float radius, Color color)
    {
#if UNITY_EDITOR
        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * direction * radius;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * direction * radius;

        Debug.DrawRay(origin, left, color);
        Debug.DrawRay(origin, right, color);

        // Dibujar el arco
        int segments = 20;
        for (int i = 0; i < segments; i++)
        {
            float t1 = (float)i / segments;
            float t2 = (float)(i + 1) / segments;

            Vector3 p1 = Quaternion.Euler(0, -angle / 2 + t1 * angle, 0) * direction * radius;
            Vector3 p2 = Quaternion.Euler(0, -angle / 2 + t2 * angle, 0) * direction * radius;

            Debug.DrawLine(origin + p1, origin + p2, color);
        }
#endif
    }
}