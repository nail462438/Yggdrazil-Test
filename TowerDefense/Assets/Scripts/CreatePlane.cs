using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (GridSystem))]
public class CreatePlane : MonoBehaviour
{
    public Material newMaterialRef;
    public bool isCollider = false;
    private GridSystem grid;

    private void Awake()
    {
        grid = GetComponent<GridSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = new GameObject("Plane");
        MeshFilter mf = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        Mesh m = new Mesh();
        m.vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(grid.width,0,0),
            new Vector3(grid.width,0,grid.height),
            new Vector3(0,0,grid.height)
        };

        m.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        mf.mesh = m;

        if (isCollider) (gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider).sharedMesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();

        mr.material = newMaterialRef;

        gameObject.layer = LayerMask.NameToLayer("Ground");
        gameObject.transform.position = new Vector3(0, 0, grid.height);
        gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
