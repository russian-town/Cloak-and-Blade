using System.Collections.Generic;
using UnityEngine;

public class CameraTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _cameraButtons;
    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        foreach (var button in _cameraButtons)
        {
            button.MainButtonClicked += OnMainButtonClicked;
            button.Open();
            button.Show();
        }
    }

    private void OnMainButtonClicked(MainButton mainButton)
    {
        if(_cameraButtons.Contains(mainButton)) 
        {
            mainButton.MainButtonClicked -= OnMainButtonClicked;
            _cameraButtons.Remove(mainButton);
            mainButton.EffectHandler.StopLightEffect();

            if(_cameraButtons.Count <= 0)
                InvokeTutorialZoneCompleteAction();
        }
    }
}
