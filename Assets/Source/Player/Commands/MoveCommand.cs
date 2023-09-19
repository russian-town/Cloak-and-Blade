using System.Collections;

public class MoveCommand : Command
{
    private Player _player;
    private PlayerMover _playerMover;
    private Gameboard _gameboard;
    private WaitPlayerClick _waitPlayerClick;
    private bool _isSelected;

    public MoveCommand(Player player, PlayerMover playerMover, Gameboard gameboard)
    {
        _player = player;
        _playerMover = playerMover;
        _gameboard = gameboard;
        _waitPlayerClick = new WaitPlayerClick(_gameboard);
    }

    protected override IEnumerator PrepareAction()
    {
        _isSelected = true;

        while (_isSelected)
        {
            yield return _waitPlayerClick;

            if (_player.TryMoveToCell(_waitPlayerClick.Cell))
                _isSelected = false;

            _player.ExecuteCurrentCommand();
        }
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return _playerMover.StartMoveCoroutine;
    }

    public override void Cancel()
    {
        _isSelected = false;
    }
}
