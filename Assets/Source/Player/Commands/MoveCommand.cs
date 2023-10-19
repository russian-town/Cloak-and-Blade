using System;
using System.Collections;
using System.Linq;

public class MoveCommand : Command, IUnmissable
{
    private float _moveSpeed;
    private float _rotationSpeed;
    private Player _player;
    private PlayerMover _playerMover;
    private PlayerView _playerView;
    private Navigator _navigator;

    public MoveCommand(Player player, PlayerMover playerMover, PlayerView playerView, Navigator navigator, float moveSpeed, float rotationSpeed)
    {
        _player = player;
        _playerMover = playerMover;
        _playerView = playerView;
        _navigator = navigator;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
    }

    protected override IEnumerator PrepareAction() 
    {
        _navigator.RefillAvailableCells(_playerMover.CurrentCell);
        _playerView.ShowAvailableCells(_navigator.AvailableCells.ToList());
        yield return null;
    }

    public override void Cancel()
    {
        _playerView.HideAvailableCells();
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        if (_player.TryMoveToCell(clickedCell, _moveSpeed, _rotationSpeed))
            yield return _player.MoveCoroutine;
    }
}
