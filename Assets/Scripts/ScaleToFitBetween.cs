using UnityEngine;

[ExecuteInEditMode]
public class ScaleToFitBetween : MonoBehaviour
{
    public Transform anchorA;
    public Transform anchorB;

    public enum Axis { X, Y, Z }
    public Axis axisToFit = Axis.Z;

    void Update()
    {
        if (anchorA == null || anchorB == null)
        {
            Debug.LogWarning("Please assign both anchors.");
            return;
        }

        // Get direction and distance between anchors
        Vector3 direction = anchorB.position - anchorA.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        direction.Normalize();

        // Get bounds size along the axis
        Bounds bounds = GetTotalBounds(gameObject);
        float currentSize = GetSizeOnAxis(bounds);

        if (currentSize <= 0f)
        {
            Debug.LogWarning("Object has zero size on the selected axis.");
            return;
        }

        // Calculate scale factor
        float scaleFactor = distance / currentSize;

        // Apply scale
        Vector3 newScale = transform.localScale;
        switch (axisToFit)
        {
            case Axis.X: newScale.x *= scaleFactor; break;
            case Axis.Y: newScale.y *= scaleFactor; break;
            case Axis.Z: newScale.z *= scaleFactor; break;
        }
        transform.localScale = newScale;

        // Recalculate bounds after scaling
        bounds = GetTotalBounds(gameObject);

        // Align object's min edge to anchorA's position
        Vector3 offset = bounds.center - transform.position;
        Vector3 minBound = bounds.min - offset;
        Vector3 position = transform.position;

        switch (axisToFit)
        {
            case Axis.X: position.x = anchorA.position.x + (transform.position.x - minBound.x); break;
            case Axis.Y: position.y = anchorA.position.y + (transform.position.y - minBound.y); break;
            case Axis.Z: position.z = anchorA.position.z + (transform.position.z - minBound.z); break;
        }

        transform.position = position;
    }

    float GetSizeOnAxis(Bounds bounds)
    {
        return axisToFit switch
        {
            Axis.X => bounds.size.x,
            Axis.Y => bounds.size.y,
            Axis.Z => bounds.size.z,
            _ => 0
        };
    }

    Bounds GetTotalBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }
}
