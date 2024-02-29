using System;
using UnityEngine;

public class DefaultLevelExit : LevelExit, ICompassTarget
{
    [SerializeField] private Treasure _treasure;

    public event Action Disabled;

    public override bool RequiredItemFound()
    {
        if(Player.ItemsInHold.FindItemInList(_treasure))
            return true;
        
        return false;
    }

    public override bool TryOpen()
    {
        InvokeLevelPassed();
        return true;

        if (Player.ItemsInHold.FindItemInList(_treasure))
        {
            InvokeLevelPassed();
            return true;
        }

        return false;
    }
}
