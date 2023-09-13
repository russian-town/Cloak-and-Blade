using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    private Player _player;
    private PlayerMover _playerMover;

    public MoveCommand(Player player, PlayerMover playerMover)
    {
        _player = player;
        _playerMover = playerMover;
    }

    protected override IEnumerator PrepareAction()
    {
        yield return null;
    }

    public override void Cancel()
    {
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        _player.TryMoveToCell(clickedCell);
        yield return _playerMover.StartMoveCoroutine;
    }
}
