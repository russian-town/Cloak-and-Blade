using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    private Player _player;

    public MoveCommand(Player player)
    {
        _player = player;
    }

    public override void Prepare()
    {

    }

    public override void Execute(Cell cell)
    {
        _player.TryMoveToCell(cell);
    }

    public override void Cancel()
    {
    }
}
