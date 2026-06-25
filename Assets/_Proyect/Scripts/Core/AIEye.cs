using UnityEngine;

public class AIEye : MonoBehaviour
{
    public float distance = 10f;

    [Range(0, 180)]
    public float angle = 30f;

    public float height = 1.0f;
    public int scanFrequency = 30;

    public LayerMask enemyLayers;
    public LayerMask occlusionLayers;

    public HealthBase ViewEnemy;
    public Transform positionEye;

    public Color meshColor = Color.red;
    public Color detectedColor = Color.green;
    public bool IsDrawGizmo = false;

    Mesh _mesh;
    float _scanInterval;
    float _scanTimer;
    int _count;
    readonly Collider[] _colliders = new Collider[50];

    float _halfHeightNeg;
    float _halfHeightPos;

    void Start()
    {
        _scanInterval = 1f / scanFrequency;
        _scanTimer = _scanInterval;
        UpdateHeightLimits();
    }

    void Update()
    {
        _scanTimer -= Time.deltaTime;

        if (_scanTimer > 0f) return;

        _scanTimer = _scanInterval;
        Scan();
    }
    void Scan()
    {
        ViewEnemy = null;

        Transform eyeTransform = positionEye != null ? positionEye : transform;

        _count = Physics.OverlapSphereNonAlloc(
            eyeTransform.position,
            distance,
            _colliders,
            enemyLayers);

        float minDistance = float.MaxValue;

        for (int i = 0; i < _count; i++)
        {
            Collider col = _colliders[i];
            if (col == null) continue;

            if (!IsInSight(col)) continue;

            HealthBase targetHealth = col.GetComponentInParent<HealthBase>();

            if (targetHealth == null)
            {
                continue;
            }

            float dist = Vector3.Distance(eyeTransform.position, col.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                ViewEnemy = targetHealth;
            }
        }
    }

    public bool IsInSight(Collider other)
    {
        Transform eyeTransform = positionEye != null ? positionEye : transform;

        Vector3 origin = eyeTransform.position;
        Vector3 dest = other.bounds.center;
        Vector3 direction = dest - origin;

        if (direction.y < _halfHeightNeg || direction.y > _halfHeightPos)
            return false;

        direction.y = 0f;

        if (direction.sqrMagnitude > distance * distance)
            return false;

        if (Vector3.Angle(direction, eyeTransform.forward) > angle)
            return false;

        return !Physics.Linecast(origin, dest, occlusionLayers);
    }

    public bool IsInSight(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        return col != null && IsInSight(col);
    }

    Mesh CreateWedgeMesh()
    {
        var wedge = new Mesh();

        const int segments = 10;
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        var vertices = new Vector3[numVertices];
        var triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 topCenter = Vector3.up * height;

        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        vertices[vert++] = bottomCenter; vertices[vert++] = bottomLeft; vertices[vert++] = topLeft;
        vertices[vert++] = topLeft; vertices[vert++] = topCenter; vertices[vert++] = bottomCenter;

        vertices[vert++] = bottomCenter; vertices[vert++] = topCenter; vertices[vert++] = topRight;
        vertices[vert++] = topRight; vertices[vert++] = bottomRight; vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2f) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            vertices[vert++] = bottomLeft; vertices[vert++] = bottomRight; vertices[vert++] = topRight;
            vertices[vert++] = topRight; vertices[vert++] = topLeft; vertices[vert++] = bottomLeft;

            vertices[vert++] = topCenter; vertices[vert++] = topLeft; vertices[vert++] = topRight;
            vertices[vert++] = bottomCenter; vertices[vert++] = bottomRight; vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
            triangles[i] = i;

        wedge.vertices = vertices;
        wedge.triangles = triangles;
        wedge.RecalculateNormals();

        return wedge;
    }

    void UpdateHeightLimits()
    {
        _halfHeightNeg = -height;
        _halfHeightPos = height;
    }

    void OnValidate()
    {
        UpdateHeightLimits();
        _mesh = CreateWedgeMesh();
    }

    void OnDrawGizmos()
    {
        if (!IsDrawGizmo) return;

        Transform eyeTransform = positionEye != null ? positionEye : transform;

        if (_mesh == null)
            _mesh = CreateWedgeMesh();

        Gizmos.color = ViewEnemy != null ? detectedColor : meshColor;
        Gizmos.DrawMesh(_mesh, eyeTransform.position, eyeTransform.rotation);

        if (ViewEnemy != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(eyeTransform.position, ViewEnemy.transform.position);
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < _count; i++)
        {
            if (_colliders[i] != null)
                Gizmos.DrawSphere(_colliders[i].transform.position, 0.5f);
        }
    }
}