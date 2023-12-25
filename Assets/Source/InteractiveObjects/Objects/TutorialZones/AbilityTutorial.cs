using System.Collections.Generic;
using UnityEngine;

public class AbilityTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _abilityButtons;

    public override void Show()
    {
        foreach (var button in _abilityButtons)
            button.Show();
    }
}
