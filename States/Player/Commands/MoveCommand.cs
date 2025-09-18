using UnityEngine;

public class MoveCommand : PlayerCommandBase
{
    private Vector3 _targetPosition;
    private bool _hasStarted = false;

    public MoveCommand(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public override bool CanExecute(PlayerStateManager player)
      => !_hasStarted && !player.IsInCarryingState(); // Verhindert Ausführung im CarryingState

    public override void Execute(PlayerStateManager player)
    {
        if (!_hasStarted)
        {
            // Auto-Drop: TreeLog fallen lassen wenn der Spieler sich bewegt
            if (player.IsInCarryingState())
            {
                Debug.Log("TreeLog wird automatisch fallen gelassen beim Bewegen");
                // Der State-Wechsel löst automatisch den Auto-Drop aus
            }
            
            player.SwitchToMoveState(_targetPosition);
            _hasStarted = true;
        }
    }

    public override bool IsComplete(PlayerStateManager player)
    {
        // Movement ist fertig wenn wir im Idle State sind und die Position erreicht haben
        if (_hasStarted && player.IsInIdleState())
            return true;

        return false;
    }
}