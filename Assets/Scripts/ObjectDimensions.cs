using UnityEngine;

public class ObjectDimensions : MonoBehaviour
{
    private void Start()
    {
        var root = GetRootParent(gameObject);
        var size = GetObjectSize(gameObject);

        Debug.Log(
            $"[Object: {gameObject.name}] [Parent: {root.name}] Width: {size.x}, Height: {size.y}, Depth: {size.z}");
    }

    // Traverse to topmost parent
    private static GameObject GetRootParent(GameObject obj)
    {
        var current = obj.transform;
        while (current.parent)
            current = current.parent;

        return current.gameObject;
    }

    // Get combined bounds of all renderers
    private static Vector3 GetObjectSize(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No Renderer found on object or its children.");
            return Vector3.zero;
        }

        var bounds = renderers[0].bounds;
        foreach (var rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        return bounds.size;
    }
}