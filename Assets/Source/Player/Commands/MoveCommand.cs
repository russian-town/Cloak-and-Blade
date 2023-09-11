public class MoveCommand : Command
{
    private Player _player;
    private PlayerMover _mover;

    public MoveCommand(Player player, PlayerMover mover)
    {
        _player = player;
        _mover = mover;
    }

    public override void Prepare()
    {

    }

    public override void Execute(Cell cell)
    {
        IsExecuting = true;
        _player.TryMoveToCell(cell);
    }

    public override void Cancel()
    {
        IsExecuting = false;
    }
}
