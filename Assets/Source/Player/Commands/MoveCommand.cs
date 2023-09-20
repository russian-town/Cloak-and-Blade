using System.Collections;
using UnityEngine;
using System.Linq;

public class MoveCommand : Command
{
    private Player _player;
    private PlayerMover _playerMover;
    private PlayerView _playerView;
    private Navigator _navigator;

    public MoveCommand(Player player, PlayerMover playerMover, PlayerView playerView, Navigator navigator)
    {
        _player = player;
        _playerMover = playerMover;
        _playerView = playerView;
        _navigator = navigator;
    }

    protected override IEnumerator PrepareAction() 
    {
        _playerView.ShowAvailableCells(_navigator.AvailableCells.ToList());
        yield return null;
    }

    public override void Cancel()
    {
        Debug.Log("Canceling.");
        _playerView.HideAvailableCells();
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        if (_player.TryMoveToCell(clickedCell))
            yield return _playerMover.StartMoveCoroutine;
    }
}
