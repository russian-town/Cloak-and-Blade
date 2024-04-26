using System.Collections;
using Source.Gameboard.Cell;
using Source.Room;

namespace Source.Player.Commands
{
    public class MoveCommand : Command, ITurnHandler
    {
        private readonly int _range;
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;
        private readonly Player _player;
        private readonly PlayerMover _playerMover;
        private readonly Navigator.Navigator _navigator;
        private readonly Gameboard.Gameboard _gameboard;
        private readonly UnityEngine.Camera _camera;
        private readonly CommandExecuter.CommandExecuter _executer;
        private Cell _cell;

        public MoveCommand(
            Player player,
            PlayerMover playerMover,
            Navigator.Navigator navigator,
            float moveSpeed,
            float rotationSpeed,
            Gameboard.Gameboard gameboard,
            CommandExecuter.CommandExecuter executer,
            int range)
            : base(executer)
        {
            _player = player;
            _playerMover = playerMover;
            _navigator = navigator;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _gameboard = gameboard;
            _camera = UnityEngine.Camera.main;
            _executer = executer;
            _range = range;
        }

        public void SetTurn(Turn turn)
        {
            if (_executer.CurrentCommand != this)
                return;

            switch (turn)
            {
                case Turn.Enemy:
                    _navigator.HideAvailableCells();
                    break;
                case Turn.Player:
                    _executer.PrepareCommand();
                    break;
            }
        }

        protected override void Cancel()
        {
            base.Cancel();
            _navigator.HideAvailableCells();
        }

        protected override IEnumerator WaitOfExecute()
        {
            WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
            yield return waitOfClickedCell;
            _cell = waitOfClickedCell.Cell;
        }

        protected override IEnumerator PrepareAction()
        {
            _navigator.RefillAvailableCellsIgnoredWalls(_playerMover.CurrentCell, _range);
            _navigator.ShowAvailableCells();
            yield return null;
        }

        protected override IEnumerator ExecuteAction()
        {
            if (_player.TryMoveToCell(_cell, _moveSpeed, _rotationSpeed))
            {
                _navigator.HideAvailableCells();
                yield return _player.MoveCoroutine;
            }
        }

        protected override void OnCommandChanged(Command command)
        {
            if (command is SkipCommand)
                command.CommandExecuted += OnCommandExecuted;

            Cancel();
        }

        private void OnCommandExecuted(Command command)
        {
            command.CommandExecuted -= OnCommandExecuted;

            if (command is SkipCommand)
                _executer.TrySwitchCommand(this);
        }
    }
}
