using UnityEngine;

[RequireComponent(typeof(Transformation))]
public class BlackGhost : Player
{
    private Transformation _transformation;
    private TransformationCommand _transformationCommand;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard, RewardedAdHandler adHandler)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler);
        _transformation = GetComponent<Transformation>();
        _transformation.Initialize(UpgradeSetter);
        _transformationCommand = new TransformationCommand(_transformation, Gameboard, Navigator, CommandExecuter, this, Mover, MoveSpeed, RotationSpeed, Range);
    }

    protected override AbilityCommand AbilityCommand()
    {
        return _transformationCommand;
    }

    protected override void TurnChanged(Turn turn) => _transformationCommand.SetTurn(turn);
}
