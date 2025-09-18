public abstract class PlayerCommandBase : IPlayerCommand
{
    protected bool _isComplete = false;

    public abstract void Execute(PlayerStateManager player);
    public abstract bool CanExecute(PlayerStateManager player);

    public virtual bool IsComplete(PlayerStateManager player) => _isComplete;

    public virtual void Cancel(PlayerStateManager player)
    {
        _isComplete = true;
    }
}