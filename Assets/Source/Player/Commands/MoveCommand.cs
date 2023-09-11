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

    public override void Prepare()
    {

    }

    public override void Cancel()
    {
    }

    protected override IEnumerator Action(Cell clickedCell)
    {
        _player.TryMoveToCell(clickedCell);
        yield return _playerMover.StartMoveCoroutine;
    }
}
