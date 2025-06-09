using UnityEngine;

public class BrickCementUnderlay : MonoBehaviour
    {
        public Material cementMaterial;
        public float cementThickness = 0.002f;
        public float cementInset = 0.001f;

    [ContextMenu("Add Cement Around Bricks")]
    public void AddCementLayers()
    {
        var bricks = GameObject.FindGameObjectsWithTag($"InsideWalls");

        foreach (var brick in bricks)
        {
            var rend = brick.GetComponent<Renderer>();
            if (!rend) continue;

            var bounds = rend.bounds;
            var center = bounds.center;
            var size = bounds.size;
            var extents = bounds.extents;
            // Bottom (Y-)
            AddCementCube(
                center - new Vector3(0, extents.y + cementThickness / 2f, 0),
                new Vector3(size.x - cementInset * 2f, cementThickness, size.z - cementInset * 2f)
            );
            
            // Top (Y+)
            AddCementCube(
                center + new Vector3(0, extents.y + cementThickness / 2f, 0),
                new Vector3(size.x - cementInset * 2f, cementThickness, size.z - cementInset * 2f)
            );

            var wall = brick.transform.parent?.parent;
            if (!wall) continue;

            var yRot = wall.eulerAngles.y;
            var deltaTo90 = Mathf.Abs(Mathf.DeltaAngle(yRot, 90f));
            var deltaTo270 = Mathf.Abs(Mathf.DeltaAngle(yRot, 270f));
            var isFacingX = deltaTo90 > 5f && deltaTo270 > 5f;

            var isRedBrick = brick.name == "Red_Brick";
            var isRedBrick12 = brick.name == "Red_Brick (12)";

            if (isFacingX)
            {
                if (!isRedBrick12 || wall.name == "Right")
                {
                    // Front (X+)
                    AddCementCube(
                        center + new Vector3(extents.x + cementThickness / 2f, 0, 0),
                        new Vector3(cementThickness, size.y - cementInset * 2f, size.z - cementInset * 2f)
                    );
                }
                if (!isRedBrick || wall.name == "Left")
                {
                    // Back (X-)
                    AddCementCube(
                        center - new Vector3(extents.x + cementThickness / 2f, 0, 0),
                        new Vector3(cementThickness, size.y - cementInset * 2f, size.z - cementInset * 2f)
                    );
                }
            }
            else
            {
                if (!isRedBrick12 ||wall.name == "Bottom")
                {
                    // Right (Z+)
                    AddCementCube(
                        center + new Vector3(0, 0, extents.z + cementThickness / 2f),
                        new Vector3(size.x - cementInset * 2f, size.y - cementInset * 2f, cementThickness)
                    );
                }

                if (!isRedBrick ||wall.name == "Front")
                {
                    // Left (Z-)
                    AddCementCube(
                        center - new Vector3(0, 0, extents.z + cementThickness / 2f),
                        new Vector3(size.x - cementInset * 2f, size.y - cementInset * 2f, cementThickness)
                    );
                }
            }
        }
    }


    private void AddCementCube(Vector3 center, Vector3 size)
    {
        var cement = new GameObject("CementUnderlay");

        // Add mesh filter and assign Unity’s built-in cube mesh
        var meshFilter = cement.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        // Add renderer and set material
        var meshRenderer = cement.AddComponent<MeshRenderer>();
        meshRenderer.material = cementMaterial;

        // Set transform
        cement.transform.position = center;
        cement.transform.localScale = size;
        cement.transform.SetParent(transform);
    }


    [ContextMenu("Clear Cement Layers")]
    public void ClearCementLayers()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("CementUnderlay"))
                DestroyImmediate(child.gameObject);
        }
    }
}
