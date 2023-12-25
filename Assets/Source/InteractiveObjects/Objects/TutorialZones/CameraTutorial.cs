using System.Collections.Generic;
using UnityEngine;

public class CameraTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _cameraButtons;

    public override void Show()
    {
        foreach (var button in _cameraButtons)
            button.Show();
    }
}
