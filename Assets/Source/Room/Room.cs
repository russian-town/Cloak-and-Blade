using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Player _player;
    private PlayerView _view;
    private List<Enemy> _enemies = new List<Enemy>();
    private Coroutine _waitEnemyTurn;
    private Turn _turn;

    public Turn Turn => _turn;

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
    }

    public void Initialize(Player player, PlayerView view)
    {
        _player = player;
        _view = view;
        _player.StepEnded += OnTurnEnded;
        _turn = Turn.Player;
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    private void OnTurnEnded()
    {
        if (_waitEnemyTurn != null)
            return;

        _turn = Turn.Enemy;
        _view.Hide();
        _waitEnemyTurn = StartCoroutine(WaitEnemiesTurn());
    }

    private IEnumerator WaitEnemiesTurn()
    {
        foreach (Enemy enemy in _enemies)
            yield return StartCoroutine(enemy.PerformMove());

        _view.Show();
        _waitEnemyTurn = null;
        _turn = Turn.Player;
    }
}

public enum Turn
{
    Enemy,
    Player
}
