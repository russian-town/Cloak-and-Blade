using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour, IEnemyTurnHandler, IPauseHandler
{
    private Player _player;
    private PlayerView _view;
    private PlayerInput _playerInput;
    private List<Enemy> _enemies = new List<Enemy>();
    private Turn _turn;
    private Coroutine _startWaitForEnemies;
    private bool _isPause;

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
        _view.Unsubscribe();
    }

    private void Update()
    {
        if (_turn == Turn.Enemy || _isPause)
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
        _view.Unsubscribe();
        _view.HideInteractiveButton();
        WaitForEnemies();
    }

    private IEnumerator WaitEnemiesTurn()
    {
        if (_enemies.Count == 0)
            yield break;

        foreach (Enemy enemy in _enemies)
            yield return StartCoroutine(enemy.PerformMove());

        _turn = Turn.Player;
        _view.Subscribe();
        _view.ShowInteravtiveButton();
        _startWaitForEnemies = null;
    }

    public void SetPause(bool isPause)
    {
        _isPause = isPause;
    }
}

public enum Turn
{
    Enemy,
    Player
}
