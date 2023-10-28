using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour, IEnemyTurnWaiter
{
    private Player _player;
    private PlayerView _view;
    private List<Enemy> _enemies = new List<Enemy>();
    private Turn _turn;
    private Coroutine _startWaitForEnemies;

    public void Unsubscribe()
    {
        _player.StepEnded -= OnTurnEnded;
        _view.Unsubscribe();
    }

    public void Initialize(Player player, PlayerView view)
    {
        _player = player;
        _view = view;
        _player.StepEnded += OnTurnEnded;
        _turn = Turn.Player;
        _player.SetTurn(_turn);
        _view.Subscribe();
        _view.ShowInteravtiveButton();
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    public Coroutine WaitForEnemies()
    {
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
            yield break;

        //for (int i = 0; i < _enemies.Count; i++)
        //{
        //    if (_enemies[i].IsFreeze)
        //        continue;

        //    if (i == _enemies.Count)
        //        yield return _enemies[i].StartPerformMove();
        //    else
        //        _enemies[i].StartPerformMove();
        //}

        foreach (Enemy enemy in _enemies)
        {
            if (enemy.IsFreeze)
                continue;

            yield return enemy.StartPerformMove();
        }

        _turn = Turn.Player;
        _player.SetTurn(_turn);
        _view.Subscribe();
        _view.ShowInteravtiveButton();
        _startWaitForEnemies = null;
    }
}

public enum Turn
{
    Enemy,
    Player
}
