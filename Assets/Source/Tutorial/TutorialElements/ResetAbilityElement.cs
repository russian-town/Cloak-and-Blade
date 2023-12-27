using UnityEngine;

public class ResetAbilityElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        _gameboard.Enable();
        player.AbilityCommand().Ability.ResetAbility();
    }
}
