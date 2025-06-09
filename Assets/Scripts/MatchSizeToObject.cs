using UnityEngine;

[ExecuteInEditMode]
public class MatchSizeToObject : MonoBehaviour
{
    [Header("Source Object (reference for size)")]
    public GameObject sourceObject;

    [Header("Axes to Match")]
    public bool matchX = true;
    public bool matchY = true;
    public bool matchZ = true;

    [Header("Match Factors (1 = exact, 0.5 = half size, etc.)")]
    [Range(0f, 10f)] public float factorX = 1f;
    [Range(0f, 10f)] public float factorY = 1f;
    [Range(0f, 10f)] public float factorZ = 1f;

    [ContextMenu("Match Size To Source")]
    public void MatchSize()
    {
        if (!sourceObject)
        {
            Debug.LogWarning("Source object is not assigned.");
            return;
        }

        var sourceBounds = GetTotalBounds(sourceObject);
        var targetBounds = GetTotalBounds(gameObject);

        if (sourceBounds.size == Vector3.zero || targetBounds.size == Vector3.zero)
        {
            Debug.LogWarning("One of the objects has zero bounds.");
            return;
        }

        var scaleFactor = new Vector3(
            matchX ? (sourceBounds.size.x / targetBounds.size.x) * factorX : 1f,
            matchY ? (sourceBounds.size.y / targetBounds.size.y) * factorY : 1f,
            matchZ ? (sourceBounds.size.z / targetBounds.size.z) * factorZ : 1f
        );

        transform.localScale = Vector3.Scale(transform.localScale, scaleFactor);

        Debug.Log($"[{name}] resized to match [{sourceObject.name}] with factors X:{factorX}, Y:{factorY}, Z:{factorZ}");
    }

    private static Bounds GetTotalBounds(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        var totalBounds = renderers[0].bounds;
        foreach (var r in renderers)
            totalBounds.Encapsulate(r.bounds);

        return totalBounds;
    }
}