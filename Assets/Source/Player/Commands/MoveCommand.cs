using System.Collections;

public class MoveCommand : Command
{
    private Player _player;
    private PlayerMover _playerMover;
    private Gameboard _gameboard;
    private WaitPlayerClick _waitPlayerClick;

    public MoveCommand(Player player, PlayerMover playerMover, Gameboard gameboard)
    {
        _player = player;
        _playerMover = playerMover;
        _gameboard = gameboard;
        _waitPlayerClick = new WaitPlayerClick(_gameboard);
    }

    protected override IEnumerator PrepareAction() 
    {
        yield return _waitPlayerClick;
        _player.ExecuteCurrentCommand();
        yield break;
    }

    public override void Cancel()
    {
    }

    protected override IEnumerator ExecuteAction()
    {
        if (_player.TryMoveToCell(_waitPlayerClick.Cell))
            yield return _playerMover.StartMoveCoroutine;
    }
}
