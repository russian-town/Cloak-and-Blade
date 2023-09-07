using Cinemachine;
using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Player _playerTemplate;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private Cell _playerSpawnCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private ParticleSystem _mouseOverCell;
    [SerializeField] private ParticleSystem _enemySightEffectTemplate;
    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private EnemySetter[] _enemySetters;

    private Player _player;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = _playerSpawner.Get(_playerSpawnCell, _playerTemplate);
        _player.Initialize(_gameboard, _playerSpawnCell, _mouseOverCell);
        _angledCamera.Follow = _player.transform;
        _angledCamera.LookAt = _player.transform;
        _straightCamera.Follow = _player.transform;
        _straightCamera.LookAt = _player.transform;

        foreach (Cell cell in _gameboard.Cells)
            cell.View.Initialize(_enemySightEffectTemplate);

        foreach(var setter in _enemySetters)
        {
            Enemy enemy = _enemySpawner.Get(setter.EnemyTemplate, setter.Destinations[0]);
            enemy.Initialize(setter.Destinations, _player, _gameboard, _musicPlayer);
        }

        _gameboard.HideGrid();
    }
}

[Serializable]
public class EnemySetter
{
    [SerializeField] private Enemy _enemyTemplate;
    [SerializeField] private Cell[] _destinations;

    public Enemy EnemyTemplate => _enemyTemplate;
    public Cell[] Destinations => _destinations;
}
