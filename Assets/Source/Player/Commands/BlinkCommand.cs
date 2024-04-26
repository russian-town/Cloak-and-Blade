using System.Collections;
using Source.Gameboard.Cell;
using Source.Player.Abiilities;
using UnityEngine;

namespace Source.Player.Commands
{
    public class BlinkCommand : AbilityCommand
    {
        private readonly Blink _blink;
        private readonly Gameboard.Gameboard _gameboard;
        private readonly UnityEngine.Camera _camera;
        private readonly Navigator.Navigator _navigator;
        private readonly CommandExecuter.CommandExecuter _executer;

        private Coroutine _executeCoroutine;
        private Cell _cell;

        public BlinkCommand(Blink blink, Gameboard.Gameboard gameboard, Navigator.Navigator navigator, CommandExecuter.CommandExecuter executer)
            : base(blink, executer)
        {
            _blink = blink;
            _gameboard = gameboard;
            _camera = UnityEngine.Camera.main;
            _navigator = navigator;
            _executer = executer;
        }

        protected override IEnumerator PrepareAction()
        {
            _blink.Prepare();
            yield break;
        }

        protected override IEnumerator WaitOfExecute()
        {
            WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
            yield return waitOfClickedCell;
            _cell = waitOfClickedCell.Cell;
        }

        protected override IEnumerator ExecuteAction()
        {
            yield return new WaitUntil(() => _blink.Cast(_cell));
        }

        protected override void Cancel()
        {
            base.Cancel();
            _blink.Cancel();

            if (_executeCoroutine != null)
            {
                _executer.StopCoroutine(_executeCoroutine);
                _executeCoroutine = null;
            }
        }
    }
}
