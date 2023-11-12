using UnityEngine;

[RequireComponent(typeof(Blink))]
public class RedGhost : Player
{
    private Blink _blink;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard, RewardedAdHandler adHandler, PlayerView playerView)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler, playerView);
        _blink = GetComponent<Blink>();
        _blink.Initialize(UpgradeSetter, playerView);
    }

    protected override AbilityCommand AbilityCommand()
    {
        return new BlinkCommand(_blink, Gameboard, Navigator, CommandExecuter);
    }
}
