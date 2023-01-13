using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Range(0, 100)]
    public int width;

    [Range(0, 100)]
    public int height;

    public float cellSize;
    public int[,] gridArray;

    // Start is called before the first frame update
    void Start()
    {
        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y, cellSize), GetWorldPosition(x, y + 1, cellSize));
                Debug.DrawLine(GetWorldPosition(x, y, cellSize), GetWorldPosition(x + 1, y, cellSize));
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height, cellSize), GetWorldPosition(width, height, cellSize));
        Debug.DrawLine(GetWorldPosition(width, 0, cellSize), GetWorldPosition(width, height, cellSize));
    }

    private Vector3 GetWorldPosition(int x, int y, float cellSize)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
}
