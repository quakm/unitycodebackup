using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerCarryingState : PlayerBaseState
{
    private TreeLog _carriedTreeLog;

    public void SetCarriedTreeLog(TreeLog treeLog)
    {
        _carriedTreeLog = treeLog;
    }

    public TreeLog GetCarriedTreeLog()
    {
        return _carriedTreeLog;
    }

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log($"Player entered Carrying state with TreeLog: {_carriedTreeLog?.name}");

        player.PlayerMovementController = player.GetComponent<PlayerMovementController>();

        // Event Listener registrieren
        player.PlayerMovementController.OnMovementCompleted += OnMovementCompleted;
        player.PlayerMovementController.OnMovementCancelled += OnMovementCancelled;
    }

    public override void UpdateState(PlayerStateManager player)
    {

        // Prüfe ob TreeLog noch existiert
        if (_carriedTreeLog == null)
        {
            player.SwitchState(player.IdleState);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Tile>(out var tile))
                {
                    player.PlayerMovementController.StartMovement(hit.point);
                }
            }
        }
    }

    private void DropTreeLog(PlayerStateManager player)
    {
        if (_carriedTreeLog != null)
        {
            // TreeLog zum Idle wechseln lassen
            var treeLogStateManager = _carriedTreeLog.GetStateManager();
            treeLogStateManager?.SwitchState(treeLogStateManager.IdleState);
        }

        // Player zurück zu Idle
        player.SwitchState(player.IdleState);
    }

    public override void ExitState(PlayerStateManager player)
    {
        // Cleanup bleibt gleich
        if (_carriedTreeLog != null)
        {
            var treeLogStateManager = _carriedTreeLog.GetStateManager();
            if (treeLogStateManager != null && treeLogStateManager.IsInCarriedState())
            {
                treeLogStateManager.SwitchState(treeLogStateManager.IdleState);
            }
        }

        _carriedTreeLog = null;
    }

    private void OnMovementCompleted()
    {

    }

    private void OnMovementCancelled()
    {

    }
}