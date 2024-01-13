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
        if (Player.ItemsInHold.FindItemInList(_treasure))
        {
            print("level passed");
            InvokeLevelPassed();
            return true;
        }
        else
        {
            print("level not passed");
        }

        return false;
    }

    protected override void Action()
    {
        throw new System.NotImplementedException();
    }
}
