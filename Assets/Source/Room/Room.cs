using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Player _player;
    private PlayerView _view;
    private List<Enemy> _enemies = new List<Enemy>();

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
    }

    public void Initialize(Player player, PlayerView view)
    {
        _player = player;
        _view = view;
        _player.StepEnded += OnTurnEnded;
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    private void OnTurnEnded()
    {
        _view.Hide();
        _player.Input.Disable();
        StartCoroutine(WaitEnemiesTurn());
    }

    private IEnumerator WaitEnemiesTurn()
    {
        foreach (Enemy enemy in _enemies)
            yield return enemy.PerformMove();

        _view.Show();
        _player.Input.Enable();
    }
}
