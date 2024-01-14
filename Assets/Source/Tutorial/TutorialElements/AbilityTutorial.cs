using System.Collections.Generic;
using UnityEngine;

public class AbilityTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _abilityButtons;
    [SerializeField] private Gameboard _gameboard;

    private Player _player;

    public override void Show(Player player)
    {
        _player = player;
        _player.Mover.MoveEnded += OnMoveEnded;
       _gameboard.Enable();

        foreach (var button in _abilityButtons)
        {
            button.Open();
            button.Show();
        }
    }

    private void OnMoveEnded()
    {
        _player.Mover.MoveEnded -= OnMoveEnded;
        InvokeTutorialZoneCompleteAction();
    }
}
