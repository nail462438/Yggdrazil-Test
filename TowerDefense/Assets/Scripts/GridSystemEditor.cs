#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (GridSystem))]
public class GridSystemEditor : Editor
{
    private void OnSceneGUI()
    {
        GridSystem grid = (GridSystem)target;

        grid.gridArray = new int[grid.width, grid.height];

        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                Handles.DrawLine(GetWorldPosition(x, y, grid.cellSize), GetWorldPosition(x, y + 1, grid.cellSize));
                Handles.DrawLine(GetWorldPosition(x, y, grid.cellSize), GetWorldPosition(x + 1, y, grid.cellSize));
            }
        }
        Handles.DrawLine(GetWorldPosition(0, grid.height, grid.cellSize), GetWorldPosition(grid.width, grid.height, grid.cellSize));
        Handles.DrawLine(GetWorldPosition(grid.width, 0, grid.cellSize), GetWorldPosition(grid.width, grid.height, grid.cellSize));
    }

    private Vector3 GetWorldPosition(int x, int y, float cellSize)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
}
#endif