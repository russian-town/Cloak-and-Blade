using UnityEngine;

[RequireComponent(typeof(Blink))]
public class RedGhost : Player
{
    [SerializeField] private UpgradeSetter _upgradeSetter;

    private Blink _blink;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView, Gameboard gameboard)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, playerView, gameboard);
        _blink = GetComponent<Blink>();
        _blink.Initialize(_upgradeSetter);
    }

    protected override AbilityCommand AbilityCommand()
    {
        return new BlinkCommand(_blink, Gameboard, Navigator, CommandExecuter);
    }
}
