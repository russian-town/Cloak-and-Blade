using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;

    public SkipCommand(Player player)
    {
        _player = player;
    }

    public override void Cancel()
    {
    }

    public override void Execute(Cell clickedCell)
    {
    }

    public override void Prepare()
    {
        _player.OnMoveEnded();
    }
}
