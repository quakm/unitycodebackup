using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speedModifier = 1f; // Für verschiedene States (Carrying = 0.7f)

    private PlayerStateManager _player;
    private PlayerMovementService _movementService;
    private Seeker _seeker;
    private List<Vector3> _pathPoints = new();
    private int _currentPathIndex;
    private float _moveProgress;
    private Vector3 _startPos;
    private Vector3 _targetPosition = Vector3.zero;
    private bool _isPathCallbackRegistered = false;
    private bool _isMoving = false;

    // Events für State Communication
    public event System.Action OnMovementStarted;
    public event System.Action OnMovementCompleted;
    public event System.Action OnMovementCancelled;

    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _player = GetComponent<PlayerStateManager>();
        _movementService = GetComponent<PlayerMovementService>();
        _seeker = GetComponent<Seeker>();
    }

    private void Start()
    {
        if (!_isPathCallbackRegistered)
        {
            _seeker.pathCallback += OnPathComplete;
            _isPathCallbackRegistered = true;
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            HandleTileMovement();
        }
    }

    public void StartMovement(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _seeker.StartPath(_player.transform.position, targetPosition);
    }

    public void StopMovement()
    {
        _isMoving = false;
        ResetMovementState();
        OnMovementCancelled?.Invoke();
    }

    public void SetSpeedModifier(float modifier)
    {
        speedModifier = modifier;
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
        {
            _isMoving = false;
            OnMovementCancelled?.Invoke();
            return;
        }

        _pathPoints = p.vectorPath;
        _currentPathIndex = 0;
        _moveProgress = 0f;

        if (_pathPoints.Count > 1)
        {
            _isMoving = true;
            StartNextMovement();
            OnMovementStarted?.Invoke();
        }
    }

    private void StartNextMovement()
    {
        _startPos = SnapToGrid(_pathPoints[_currentPathIndex]);
        _targetPosition = SnapToGrid(_pathPoints[_currentPathIndex + 1]);

        // Optional: Drehung vor Bewegung
        _player.transform.rotation = Quaternion.LookRotation((_targetPosition - _startPos).normalized);
        _moveProgress = 0f;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            position.y,
            Mathf.Round(position.z)
        );
    }

    private void HandleTileMovement()
    {
        // Speed Modifier berücksichtigen (für Carrying State etc.)
        float adjustedTimePerTile = _movementService.TimePerTile / speedModifier;

        _moveProgress += Time.deltaTime / adjustedTimePerTile;
        _moveProgress = Mathf.Clamp01(_moveProgress);

        // Lineare Bewegung zwischen Tile-Mittelpunkten
        _player.transform.position = Vector3.Lerp(_startPos, _targetPosition, _moveProgress);

        if (_moveProgress >= 1f)
        {
            _currentPathIndex++;
            if (_currentPathIndex < _pathPoints.Count - 1)
            {
                StartNextMovement();
            }
            else
            {
                // Bewegung beendet
                _player.transform.position = SnapToGrid(_targetPosition);
                _isMoving = false;
                ResetMovementState();
                OnMovementCompleted?.Invoke();
            }
        }
    }

    private void ResetMovementState()
    {
        // Cleanup aber Callback nicht entfernen (wird nur einmal registriert)
        _pathPoints.Clear();
        _currentPathIndex = 0;
        _moveProgress = 0f;
    }

    private void OnDestroy()
    {
        if (_isPathCallbackRegistered)
        {
            _seeker.pathCallback -= OnPathComplete;
        }
    }
}