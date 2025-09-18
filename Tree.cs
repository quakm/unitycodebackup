using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private TreeController _treeController;

    [Header("Tree Stats")]
    public float MaxHealth;

    public float CurrentHealth;

    [Header("Leveling")]
    [SerializeField]
    private int _requiredLevelToCut = 1;

    [SerializeField]
    public float XPDropPerCut = 5f;

    [Header("Other")]
    [SerializeField]
    public float RespawnTime = 15f;

    [SerializeField]
    private GameObject _destroyedTreePrefab;

    [SerializeField]
    private GameObject _treeLogPrefab;

    public bool IsDestroyed;

    private int _tileSize;

    [SerializeField]
    public LayerMask ObstacleLayerMask;

    public Vector3 TreePosition { get; private set; }

    public List<Vector3> InteractionTiles = new();

    private void Start()
    {

        ObstacleLayerMask = LayerMask.GetMask("Obstacle");

        _tileSize = GridManager.Instance.UnityGridSize;
        _treeController = TreeController.Instance;
        TreePosition = transform.position;
        CurrentHealth = MaxHealth;

        CollectInteractionTiles();
    }

    private void CollectInteractionTiles()
    {
        Vector3 treePos = new(transform.position.x, 0.5f, transform.position.z);


        Vector3[] potentialPositions = new Vector3[]
        {
            treePos + Vector3.right * _tileSize,
            treePos + Vector3.left * _tileSize,
            treePos + Vector3.forward * _tileSize,
            treePos + Vector3.back * _tileSize
        };

        for (int i = 0; i < potentialPositions.Length; i++)
        {
            Vector3 pos = potentialPositions[i];

            if (IsTileFree(pos))
                InteractionTiles.Add(pos);

        }
    }

    private bool IsTileFree(Vector3 position)
    {
        Vector3 halfExtents = new(
            _tileSize * 0.4f,   // 40% der Tile-Breite
            _tileSize * 0.5f,   // H�he f�r 3D-Objekte  
            _tileSize * 0.4f    // 40% der Tile-Tiefe
        );


        Collider[] overlapping = Physics.OverlapBox(position, halfExtents, Quaternion.identity, ObstacleLayerMask);

        if (overlapping.Length > 0)
        {
            for (int i = 0; i < overlapping.Length; i++)
            {
                Collider col = overlapping[i];

                // WICHTIG: Pr�fe ob es der Baum selbst ist
                if (col.gameObject ==  gameObject || col.transform.IsChildOf(transform))
                    continue;

                return false;
            }

            return true;
        }

        return true;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Vector3 tileSize = new(_tileSize, 1f, _tileSize);

        foreach(var interactionTile in InteractionTiles)
        {
            Gizmos.DrawWireCube(interactionTile, tileSize);
        }
    }

    public int GetRequiredLevelToCut()
        => _requiredLevelToCut;

    public GameObject TreeLogPrefab => _treeLogPrefab;
}
