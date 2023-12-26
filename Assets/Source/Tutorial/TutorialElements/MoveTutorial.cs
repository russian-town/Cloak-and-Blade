using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();
    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        _gameboard.Enable();

        foreach (var button in _mainButtons)
        {
            button.Open();
            button.Show();
        }
    }
}
