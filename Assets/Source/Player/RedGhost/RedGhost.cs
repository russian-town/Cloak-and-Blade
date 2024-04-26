using Source.Gameboard.Cell;
using Source.Player.Abiilities;
using Source.Player.Commands;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using Source.Room;
using Source.Yandex;
using UnityEngine;

namespace Source.Player.RedGhost
{
    [RequireComponent(typeof(Blink))]
    public class RedGhost : Player
    {
        private Blink _blink;

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
            _blink = GetComponent<Blink>();
            _blink.Initialize(UpgradeSetter, playerView);
        }

        public override AbilityCommand GetAbilityCommand()
        {
            return new BlinkCommand(_blink, Gameboard, Navigator, CommandExecuter);
        }
    }
}
