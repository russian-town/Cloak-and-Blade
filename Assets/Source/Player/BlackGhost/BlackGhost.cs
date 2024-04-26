using Source.Gameboard.Cell;
using Source.Player.Abiilities;
using Source.Player.Commands;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using Source.Room;
using Source.Yandex;
using UnityEngine;

namespace Source.Player.BlackGhost
{
    [RequireComponent(typeof(Transformation))]
    public class BlackGhost : Player
    {
        private Transformation _transformation;
        private TransformationCommand _transformationCommand;

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
            _transformation = GetComponent<Transformation>();
            _transformation.Initialize(UpgradeSetter, playerView);
            _transformationCommand = new TransformationCommand(
                _transformation,
                Gameboard,
                Navigator,
                CommandExecuter,
                this,
                Mover,
                MoveSpeed,
                RotationSpeed,
                Range);
        }

        public override AbilityCommand GetAbilityCommand()
            => _transformationCommand;

        protected override void TurnChanged(Turn turn)
            => _transformationCommand.SetTurn(turn);
    }
}
