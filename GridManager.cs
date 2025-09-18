using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance;

    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int _unityGridSize;

    public int UnityGridSize { get { return _unityGridSize; } }

    private readonly Dictionary<Vector2Int, GridNode> _grid = new();

    private void Awake()
    {
        Instance = this;

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector2Int cords = new(x, y);
                _grid.Add(cords, new GridNode(cords));
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / _unityGridSize),
            Mathf.FloorToInt(worldPosition.z / _unityGridSize) // Z-Achse für 3D
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            (gridPosition.x * _unityGridSize),
            0,
            (gridPosition.y * _unityGridSize)
        );
    }

    public GridNode GetNode(Vector2Int coords)
    {
        return _grid.TryGetValue(coords, out GridNode node) ? node : null;
    }

    //void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;

    //    Gizmos.color = Color.green;
    //    foreach (var entry in _grid)
    //    {
    //        Vector3 pos = GridToWorld(entry.Key);
    //        Gizmos.DrawWireCube(pos, Vector3.one * 0.9f);
    //    }

    //    var graph = AstarPath.active.data.gridGraph;
    //    Gizmos.color = Color.red;
    //    for (int x = 0; x < graph.width; x++)
    //    {
    //        for (int z = 0; z < graph.depth; z++)
    //        {
    //            GraphNode node = graph.GetNode(x, z);
    //            Gizmos.DrawSphere((Vector3)node.position, 0.2f);
    //        }
    //    }
    //}
}