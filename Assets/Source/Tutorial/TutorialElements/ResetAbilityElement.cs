using UnityEngine;

public class ResetAbilityElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private PlayerView _playerView;

    public override void Show(Player player)
    {
        _gameboard.Enable();
        player.AbilityCommand().Ability.ResetAbility();
        _playerView.ResetAbilityIcon();
    }
}
