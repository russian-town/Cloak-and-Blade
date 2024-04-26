using System;
using Source.Player.PlayerUI.ArrowPointer;
using UnityEngine;

namespace Source.InteractiveObjects.Objects.LevelExit
{
    public class DefaultLevelExit : LevelExit, ICompassTarget
    {
        [SerializeField] private Treasure.Treasure _treasure;

        public event Action Disabled;

        public override bool RequiredItemFound()
        {
            if (Player.ItemsInHold.FindItemInList(_treasure))
                return true;

            return false;
        }

        public override bool TryOpen()
        {
            if (Player.ItemsInHold.FindItemInList(_treasure))
            {
                InvokeExitOpened();
                return true;
            }

            return false;
        }
    }
}
