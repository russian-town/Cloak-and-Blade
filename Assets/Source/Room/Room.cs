using System.Collections;
using System.Collections.Generic;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using UnityEngine;

namespace Source.Room
{
    public class Room : MonoBehaviour, IEnemyTurnWaiter
    {
        [SerializeField] [Range(0f, 1f)] private float _delay;

        private Player.Player _player;
        private PlayerView _view;
        private List<Enemy.Enemy> _enemies = new List<Enemy.Enemy>();
        private Turn _turn;
        private Coroutine _startWaitForEnemies;
        private Hourglass _hourglass;

        public void Unsubscribe()
        {
            _player.StepEnded -= OnTurnEnded;
            _view.Unsubscribe();
        }

        public void Initialize(Player.Player player, PlayerView view, Hourglass hourglass)
        {
            _player = player;
            _view = view;
            _hourglass = hourglass;
            _hourglass.Initialaze();
            _player.StepEnded += OnTurnEnded;
            _turn = Turn.Player;
            _player.SetTurn(_turn);
            _view.Subscribe();
            _view.ShowInteractiveButton();
            WaitForEnemies();
        }

        public void AddEnemy(Enemy.Enemy enemy) => _enemies.Add(enemy);

        public void RemoveEnemies() => _enemies.Clear();

        public Coroutine WaitForEnemies()
        {
            if (_startWaitForEnemies != null)
                return null;

            _startWaitForEnemies = StartCoroutine(WaitEnemiesTurn());
            return _startWaitForEnemies;
        }

        private void OnTurnEnded()
        {
            if (_startWaitForEnemies != null)
                return;

            _turn = Turn.Enemy;
            _player.SetTurn(_turn);
            _view.Unsubscribe();
            _view.HideInteractiveButton();
            WaitForEnemies();
        }

        private IEnumerator WaitEnemiesTurn()
        {
            if (_enemies.Count == 0)
            {
                yield return _hourglass.StartShow();
                yield return _hourglass.StartHide();
                SetPlayerTurn();
                _startWaitForEnemies = null;
                yield break;
            }

            yield return _hourglass.StartShow();
            WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);

            foreach (Enemy.Enemy enemy in _enemies)
            {
                if (enemy.IsFrozen)
                    continue;

                if (_enemies.IndexOf(enemy) == _enemies.Count)
                {
                    yield return enemy.StartPerformMove();
                }
                else
                {
                    enemy.StartPerformMove();
                    yield return waitForSeconds;
                }
            }

            yield return _hourglass.StartHide();
            SetPlayerTurn();
            _startWaitForEnemies = null;
        }

        private void SetPlayerTurn()
        {
            _turn = Turn.Player;
            _view.Subscribe();
            _view.ShowInteractiveButton();
            _player.SetTurn(_turn);
        }
    }
}
