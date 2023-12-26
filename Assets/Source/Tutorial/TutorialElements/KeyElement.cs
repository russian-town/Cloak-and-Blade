using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        _gameboard.Enable();
    }
}
