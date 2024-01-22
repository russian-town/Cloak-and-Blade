using System.Collections.Generic;
using UnityEngine;

public class ResetAbilityElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private List<MainButton> _abilityButtons;
    [SerializeField] private Battery _battery;

    private Player _player;

    public override void Show(Player player)
    {
        _player = player;
        _player.Mover.MoveEnded += OnMoveEnded;
        _gameboard.Enable();
        player.AbilityCommand().Ability.ResetAbility();
        _playerView.ResetAbilityIcon();
        _battery.Enable();

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
