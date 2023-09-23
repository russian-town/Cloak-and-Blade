using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Player _player;
    private PlayerView _view;
    private PlayerInput _playerInput;
    private List<Enemy> _enemies = new List<Enemy>();
    private Turn _turn;

    public Coroutine WaitForEnemies { get; private set; }
    public Turn Turn => _turn;

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
        _view.Unsubscribe();
    }

    private void Update()
    {
        if (_turn == Turn.Enemy)
            return;

        _playerInput.GameUpdate();
    }

    public void Initialize(Player player, PlayerView view, PlayerInput playerInput)
    {
        _player = player;
        _view = view;
        _player.StepEnded += OnTurnEnded;
        _turn = Turn.Player;
        _playerInput = playerInput;
        _view.Subscribe();
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    private void OnTurnEnded()
    {
        if (WaitForEnemies != null)
            return;

        _view.Unsubscribe();
        _turn = Turn.Enemy;
        _view.Hide();
        WaitForEnemies = StartCoroutine(WaitEnemiesTurn());
    }

    private IEnumerator WaitEnemiesTurn()
    {
        foreach (Enemy enemy in _enemies)
            yield return StartCoroutine(enemy.PerformMove());

        _view.Show();
        WaitForEnemies = null;
        _turn = Turn.Player;
        _view.Subscribe();
    }
}

public enum Turn
{
    Enemy,
    Player
}
