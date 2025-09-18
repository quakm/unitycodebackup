using System.Collections.Generic;
using UnityEngine;


public class PlayerStateManager : MonoBehaviour
{
    public PlayerInputHandler InputHandler;
    public PlayerSkills PlayerSkills;
    public PlayerInventory PlayerInventory;
    public PlayerMovementController PlayerMovementController;

    private PlayerBaseState _currentState;

    public PlayerIdleState IdleState = new();
    public PlayerMoveState MoveState = new();
    public PlayerWoodcuttingState WoodcuttingState = new();
    public PlayerCarryingState CarryingState = new();

    private readonly Queue<IPlayerCommand> _commandQueue = new();

    public int CommandCounts;

    private IPlayerCommand _currentCommand;

    public bool IsInIdleState() => _currentState == IdleState;
    public bool IsInMoveState() => _currentState == MoveState;
    public bool IsInWoodcuttingState() => _currentState == WoodcuttingState;
    public bool IsInCarryingState() => _currentState == CarryingState;

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        PlayerSkills = transform.parent.GetComponent<Player>().PlayerSkills;
        PlayerInventory = transform.parent.GetComponent<Player>().PlayerInventory;

        _currentState = IdleState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        CommandCounts = _commandQueue.Count;

        _currentState.UpdateState(this);
        ProcessCommandQueue();
    }

    private void ProcessCommandQueue()
    {
        if (_currentCommand == null && _commandQueue.Count > 0)
            _currentCommand = _commandQueue.Dequeue();

        if (_currentCommand != null)
        {
            if (_currentCommand.CanExecute(this))
            {
                Debug.LogWarning($"{_currentCommand.GetType().Name} is executed!");
                _currentCommand.Execute(this);
            }

            if (_currentCommand.IsComplete(this))
                _currentCommand = null;
            
        }
    }

    public void AddCommand(IPlayerCommand newCommand)
    {
        _commandQueue.Enqueue(newCommand);
    }

    public void ClearCommands()
    {
        _currentCommand?.Cancel(this);
        _currentCommand = null;
        _commandQueue.Clear();
    }

    public void ReplaceCommands(params IPlayerCommand[] commands)
    {
        ClearCommands();

        foreach (var command in commands)
            AddCommand(command);
        
    }

    public void SwitchState(PlayerBaseState state)
    {
        _currentState?.ExitState(this);

        _currentState = state;
        state.EnterState(this);
    }

    public void SwitchToMoveState(Vector3 targetPosition)
    {
        MoveState.SetTargetPosition(targetPosition);
        SwitchState(MoveState);
    }

    public void SwitchToWoodcuttingState(Tree tree)
    {
        WoodcuttingState.SetTargetTree(tree);
        SwitchState(WoodcuttingState);
    }

    public void SwitchToCarryingState(TreeLog treeLog)
    {
        CarryingState.SetCarriedTreeLog(treeLog);
        SwitchState(CarryingState);
    }

}

