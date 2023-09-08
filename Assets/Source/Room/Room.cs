using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Player _player;
    private List<Enemy> _enemies = new List<Enemy>();

    private void OnDisable()
    {
        _player.StepEnded -= OnTurnEnded;
    }

    public void Initialize(Player player)
    {
        _player = player;
        _player.StepEnded += OnTurnEnded;
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);

    private void OnTurnEnded()
    {
        _player.DisableInput();
        StartCoroutine(WaitEnemiesTurn());
    }

    private IEnumerator WaitEnemiesTurn()
    {
        foreach (Enemy enemy in _enemies)
            yield return enemy.PerformMove();

        _player.EnableInput();
    }
}
