using System;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private Vector3 _targetPosition;
    private PlayerStateManager _player;

    public void SetTargetPosition(Vector3 position)
    {
        // Einfach die Zielposition setzen, wird beim Enter verwendet
        _targetPosition = position;
    }

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered PlayerMoveState");
        _player = player;

        player.PlayerMovementController = player.GetComponent<PlayerMovementController>();

        // Event Listener registrieren
        player.PlayerMovementController.OnMovementCompleted += OnMovementCompleted;
        player.PlayerMovementController.OnMovementCancelled += OnMovementCancelled;

        // Bewegung starten
        player.PlayerMovementController.StartMovement(_targetPosition);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Nur Input handling, Bewegung läuft in MovementController
        player.InputHandler.HandleMouseClick();
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit PlayerMoveState");

        // Events abmelden
        if (player.PlayerMovementController != null)
        {
            player.PlayerMovementController.OnMovementCompleted -= OnMovementCompleted;
            player.PlayerMovementController.OnMovementCancelled -= OnMovementCancelled;
        }
    }

    private void OnMovementCompleted()
    {
        _player.SwitchState(_player.IdleState);
    }

    private void OnMovementCancelled()
    {
        _player.SwitchState(_player.IdleState);
    }


    //private PlayerStateManager _player;

    //private PlayerMovementService _movementService;
    //private Seeker _seeker;
    //private List<Vector3> _pathPoints = new();
    //private int _currentPathIndex;
    //private float _moveProgress;
    //private Vector3 _startPos;
    //private Vector3 _targetPosition = Vector3.zero;

    //private bool _isPathCallbackRegistered = false;

    //public override void EnterState(PlayerStateManager player)
    //{
    //    _player = player;
    //    _movementService = player.GetComponent<PlayerMovementService>();
    //    _seeker = player.GetComponent<Seeker>();

    //    if (!_isPathCallbackRegistered)
    //    {
    //        _seeker.pathCallback += OnPathComplete;
    //        _isPathCallbackRegistered = true;
    //    }

    //    Debug.Log("Started Player Move State");

    //    StartMovement(_targetPosition);
    //}

    //public override void UpdateState(PlayerStateManager player)
    //{
    //    HandleTileMovement();

    //    player.InputHandler.HandleMouseClick();
    //}

    //public void StartMovement(Vector3 pointer)
    //{
    //    _seeker.StartPath(_player.transform.position, pointer);
    //}

    //public void SetTargetPosition(Vector3 position)
    //{
    //    _targetPosition = position;
    //}

    //private void OnPathComplete(Path p)
    //{
    //    if (p.error)
    //    {
    //        _player.SwitchState(_player.IdleState);
    //        return;
    //    }

    //    _pathPoints = p.vectorPath;
    //    _currentPathIndex = 0;
    //    _moveProgress = 0f;

    //    if (_pathPoints.Count > 1)
    //        StartNextMovement();

    //}

    //private void StartNextMovement()
    //{
    //    _startPos = SnapToGrid(_pathPoints[_currentPathIndex]);
    //    _targetPosition = SnapToGrid(_pathPoints[_currentPathIndex + 1]);

    //    // Optional: Drehung vor Bewegung
    //    _player.transform.rotation = Quaternion.LookRotation((_targetPosition - _startPos).normalized);

    //    _moveProgress = 0f;
    //}

    //private Vector3 SnapToGrid(Vector3 position)
    //{
    //    return new Vector3(
    //        Mathf.Round(position.x), // Rundet zur nächsten ganzen Zahl
    //        position.y,
    //        Mathf.Round(position.z)  // Rundet zur nächsten ganzen Zahl
    //    );
    //}

    //private void HandleTileMovement()
    //{
    //    _moveProgress += Time.deltaTime / _movementService.TimePerTile;
    //    _moveProgress = Mathf.Clamp01(_moveProgress);

    //    // Lineare Bewegung zwischen Tile-Mittelpunkten
    //    _player.transform.position = Vector3.Lerp(_startPos, _targetPosition, _moveProgress);

    //    if (_moveProgress >= 1f)
    //    {
    //        _currentPathIndex++;
    //        if (_currentPathIndex < _pathPoints.Count - 1)
    //        {
    //            StartNextMovement();
    //        }
    //        else
    //        {
    //            _player.transform.position = SnapToGrid(_targetPosition);
    //            _player.SwitchState(_player.IdleState);
    //            ResetMovementState();
    //        }
    //    }
    //}


    //private void ResetMovementState()
    //{
    //    _seeker.pathCallback -= OnPathComplete;
    //    _isPathCallbackRegistered = false;
    //}

    //public override void ExitState(PlayerStateManager player)
    //{

    //}
}

