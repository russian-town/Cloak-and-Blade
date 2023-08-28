using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Enemy _enemyTemplate;
    [SerializeField] private Player _playerTemplate;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private Cell _playerSpawnCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Cell[] _enemySpawnCells;

    private Enemy _enemy;
    private Player _player;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = _playerSpawner.Get(_playerSpawnCell, _playerTemplate);
        _player.Initialize(_gameboard, _playerSpawnCell);

        foreach(var cell in _enemySpawnCells)
        {
            _enemy = _enemySpawner.Get(cell, _enemyTemplate);
            _enemy.Initialize(cell, _player, _gameboard);
        }
    }
}
