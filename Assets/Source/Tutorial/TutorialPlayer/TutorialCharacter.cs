using Source.Gameboard.Cell;
using Source.Player.Commands;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using Source.Room;
using Source.Tutorial.TutorialZones;
using Source.Yandex;

namespace Source.Tutorial.TutorialPlayer
{
    public class TutorialCharacter : Player.Player
    {
        private TutorialBlink _tutorialBlink;

        public override void Initialize(
            Cell startCell,
            Hourglass hourglass,
            IEnemyTurnWaiter enemyTurnHandler,
            Gameboard.Gameboard gameboard,
            RewardedAdHandler adHandler,
            PlayerView playerView,
            Battery battery)
        {
            base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler, playerView, battery);
            _tutorialBlink = GetComponent<TutorialBlink>();
            _tutorialBlink.Initialize(UpgradeSetter, playerView);
        }

        public void SetSide(Side side) => _tutorialBlink.SetSide(side);

        public override AbilityCommand GetAbilityCommand()
        {
            return new BlinkCommand(_tutorialBlink, Gameboard, Navigator, CommandExecuter);
        }
    }
}
