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
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private Cell[] _enemySpawnCells;

    private Enemy _enemy;
    private Player _player;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gameboard.SetDefaultCellColor();
        _player = _playerSpawner.Get(_playerSpawnCell, _playerTemplate);
        _player.Initialize(_gameboard, _playerSpawnCell);
        _angledCamera.Follow = _player.transform;
        _angledCamera.LookAt = _player.transform;
        _straightCamera.Follow = _player.transform;
        _straightCamera.LookAt = _player.transform;

        foreach(var cell in _enemySpawnCells)
        {
            _enemy = _enemySpawner.Get(cell, _enemyTemplate);
            _enemy.Initialize(cell, _player, _gameboard);
        }
    }
}
