using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Treasure _treasure;

    public override void Show(Player player)
    {
        _treasure.Opened += OnTreasureOpened;
        _gameboard.Enable();
    }

    private void OnTreasureOpened()
    {
        _treasure.Opened -= OnTreasureOpened;
        InvokeTutorialZoneCompleteAction();
    }
}
