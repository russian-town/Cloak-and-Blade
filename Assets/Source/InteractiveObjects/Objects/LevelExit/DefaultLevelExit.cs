using UnityEngine;

public class DefaultLevelExit : LevelExit
{
    [SerializeField] private Treasure _treasure;

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
