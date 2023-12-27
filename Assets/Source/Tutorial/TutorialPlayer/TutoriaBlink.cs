using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoriaBlink : Blink
{
    private Side _side;

    public void SetSide(Side side) => _side = side;

    public override void RefillNavigatorCells()
    {
        if (_side == Side.East)
            Navigator.RefillEastAvailableCells(Player.CurrentCell, BlinkRange);
        else
            Navigator.RefillWestAvailableCells(Player.CurrentCell, BlinkRange);
    }
}
