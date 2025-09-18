using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerMovementService : MonoBehaviour
{
    public float TimePerTile = 0.3f;
    public Seeker Seeker { get; private set; }
    public List<Vector3> PathPoints { get; private set; } = new();

    private void Start()
    {
        Seeker = GetComponent<Seeker>();
    }
}
