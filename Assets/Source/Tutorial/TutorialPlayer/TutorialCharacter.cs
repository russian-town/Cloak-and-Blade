public class TutorialCharacter : Player
{
    private TutorialBlink _tutorialBlink;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard, RewardedAdHandler adHandler, PlayerView playerView, Battery battery)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler, playerView, battery);
        _tutorialBlink = GetComponent<TutorialBlink>();
        _tutorialBlink.Initialize(UpgradeSetter, playerView);
    }

    public void SetSide(Side side) => _tutorialBlink.SetSide(side);

    public override AbilityCommand AbilityCommand()
    {
        return new BlinkCommand(_tutorialBlink, Gameboard, Navigator, CommandExecuter);
    }
}
