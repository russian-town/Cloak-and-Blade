using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Room : MonoBehaviour, IEnemyTurnWaiter
{
    private Player _player;
    private PlayerView _view;
    private List<Enemy> _enemies = new List<Enemy>();
    private Turn _turn;
    private Coroutine _startWaitForEnemies;
    private Hourglass _hourglass;

    public void Unsubscribe()
    {
        _player.StepEnded -= OnTurnEnded;
        _view.Unsubscribe();
    }

    public void Initialize(Player player, PlayerView view, Hourglass hourglass)
    {
        _player = player;
        _view = view;
        _hourglass = hourglass;
        _hourglass.Initialaze();
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

        yield return _hourglass.StartShow();

        foreach (Enemy enemy in _enemies)
        {
            if (enemy.IsFreeze)
                continue;

            yield return enemy.StartPerformMove();
        }

        yield return _hourglass.StartHide();
        _turn = Turn.Player;
        _view.Subscribe();
        _view.ShowInteravtiveButton();
        _startWaitForEnemies = null;
        _player.SetTurn(_turn);
    }
}

public enum Turn
{
    Enemy,
    Player
}
