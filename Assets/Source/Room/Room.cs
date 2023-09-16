using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Player _player;
    private PlayerView _view;
    private List<Enemy> _enemies = new List<Enemy>();
    private Coroutine _waitEnemyTurn;

    public bool CanMove { get; private set; }

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
    }

    public void Initialize(Player player, PlayerView view)
    {
        _player = player;
        _view = view;
        _player.StepEnded += OnTurnEnded;
        CanMove = true;
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    private void OnTurnEnded()
    {
        if (_waitEnemyTurn != null)
            return;

        CanMove = false;
        _view.Hide();
        _waitEnemyTurn = StartCoroutine(WaitEnemiesTurn());
    }

    private IEnumerator WaitEnemiesTurn()
    {
        foreach (Enemy enemy in _enemies)
            yield return StartCoroutine(enemy.PerformMove());

        _view.Show();
        _waitEnemyTurn = null;
        CanMove = true;
    }
}
