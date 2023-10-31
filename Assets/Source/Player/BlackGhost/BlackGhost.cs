using UnityEngine;

[RequireComponent(typeof(Transformation))]
public class BlackGhost : Player
{
    private Transformation _transformation;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard);
        _transformation = GetComponent<Transformation>();
        _transformation.Initialize(UpgradeSetter);
    }

    protected override AbilityCommand AbilityCommand()
    {
        return new TransformationCommand(_transformation, Gameboard, Navigator, CommandExecuter, this, Mover, MoveSpeed, RotationSpeed, Range);
    }
}
