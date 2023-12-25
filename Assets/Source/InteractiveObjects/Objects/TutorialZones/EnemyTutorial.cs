using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();

    public override void Show()
    {
        foreach (var button in _mainButtons)
            button.Show();
    }
}
