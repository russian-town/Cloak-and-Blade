using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();

    public override void Show()
    {
        foreach (var button in _mainButtons)
        {
            button.Open();
            button.Show();
        }
    }
}
