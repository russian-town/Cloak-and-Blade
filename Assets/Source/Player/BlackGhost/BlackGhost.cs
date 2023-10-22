using UnityEngine;

[RequireComponent(typeof(Transformation))]
public class BlackGhost : Player
{
    [SerializeField] private UpgradeSetter _upgradeSetter;

    private Transformation _transformation;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, PlayerView playerView, Gameboard gameboard)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, playerView, gameboard);
        _transformation = GetComponent<Transformation>();
        _transformation.Initialize(_upgradeSetter);
    }

    protected override AbilityCommand AbilityCommand()
    {
        return new TransformationCommand(_transformation, Gameboard, Navigator, CommandExecuter);
    }
}
