//using Pathfinding;
//using UnityEngine;
//using System.Collections.Generic;

//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField]
//    private float _timePerTile = 0.3f;

//    private Seeker _seeker;
//    private List<Vector3> _pathPoints = new();
//    private int _currentPathIndex;
//    private float _moveProgress;
//    private Vector3 _startPos;
//    private Vector3 _targetPos;

//    public bool IsMoving { get; private set; }


//    private void Start()
//    {
//        _seeker = GetComponent<Seeker>();
//        _seeker.pathCallback += OnPathComplete;
//    }

//    private void OnPathComplete(Path p)
//    {
//        if (p.error) return;

//        _pathPoints = p.vectorPath;
//        _currentPathIndex = 0;
//        _moveProgress = 0f;

//        if (_pathPoints.Count > 1) // Mindestens 1 Tile Bewegung
//        {
//            StartNextMovement();
//        }
//    }

//    public void StartMovement(Vector3 pointer)
//    {
//        _seeker.StartPath(transform.position, pointer);
//    }

//    private void Update()
//    {
//        if (IsMoving)
//        {
//            HandleTileMovement();
//        }
//    }

//    private void StartNextMovement()
//    {
//        _startPos = SnapToGrid(_pathPoints[_currentPathIndex]);
//        _targetPos = SnapToGrid(_pathPoints[_currentPathIndex + 1]);

//        // Optional: Drehung vor Bewegung
//        transform.rotation = Quaternion.LookRotation((_targetPos - _startPos).normalized);

//        IsMoving = true;
//        _moveProgress = 0f;
//    }

//    private void HandleTileMovement()
//    {
//        _moveProgress += Time.deltaTime / _timePerTile;
//        _moveProgress = Mathf.Clamp01(_moveProgress);

//        // Lineare Bewegung zwischen Tile-Mittelpunkten
//        transform.position = Vector3.Lerp(_startPos, _targetPos, _moveProgress);

//        if (_moveProgress >= 1f)
//        {
//            _currentPathIndex++;
//            if (_currentPathIndex < _pathPoints.Count - 1)
//            {
//                StartNextMovement();
//            }
//            else
//            {
//                // Finales Snapping
//                transform.position = SnapToGrid(_targetPos);
//                IsMoving = false;

//            }
//        }
//    }

//    private Vector3 SnapToGrid(Vector3 position)
//    {
//        return new Vector3(
//            Mathf.Floor(position.x),
//            position.y,
//            Mathf.Floor(position.z)
//        );
//    }

//    private void OnDestroy()
//    {
//        if (_seeker != null)
//            _seeker.pathCallback -= OnPathComplete;
//    }

//    private void OnDrawGizmos()
//    {
//        if (IsMoving)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(_startPos, _targetPos);
//            Gizmos.DrawWireCube(_targetPos, Vector3.one * 0.9f);
//        }
//    }
//}
