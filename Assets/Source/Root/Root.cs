using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Player _playerTemplate;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private InputView _inputView;
    [SerializeField] private Cell _playerSpawnCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private ParticleSystem _abilityRangeTemplate;
    [SerializeField] private EnemySetter[] _enemySetters;
    [SerializeField] private Room _room;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private AnimationClip _hourglassAnimation;
    [SerializeField] private Animator _hourglassAnimator;
    [SerializeField] private CanvasGroup _hourglass;
    [SerializeField] private InteractiveObject[] _interactiveObjects;
    [SerializeField] private EnemyZoneDrawer _enemyZoneDrawerTemplate;
    [SerializeField] private GhostSpawner _spawner;
    [SerializeField] private Game _game;

    private Player _player;
    private Pause _pause;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = (Player)_spawner.Get(_playerSpawnCell, _playerTemplate);
        _player.Initialize(_playerSpawnCell, _hourglassAnimation, _hourglassAnimator, _hourglass, _room, _playerView);
        _playerInput.Initialize(_camera, _gameboard, _player);
        _playerView.Initialize(_player);
        _angledCamera.Follow = _player.transform;
        _angledCamera.LookAt = _player.transform;
        _straightCamera.Follow = _player.transform;
        _straightCamera.LookAt = _player.transform;

        foreach (Cell cell in _gameboard.Cells)
            cell.View.Initialize(_abilityRangeTemplate);

        foreach (var interactiveObject in _interactiveObjects)
            interactiveObject.Initialize(_player);

        _pause = new Pause(new List<IPauseHandler> {_inputView, _room, _playerView, _player });

        foreach (var setter in _enemySetters)
        {
            EnemyZoneDrawer zoneDrawer = Instantiate(_enemyZoneDrawerTemplate, new Vector3(0, 0.1f, 0), Quaternion.identity);
            Enemy enemy = (Enemy)_spawner.Get(setter.Destinations[0], setter.EnemyTemplate);
            enemy.Initialize(setter.Destinations, _player, _gameboard, zoneDrawer);
            _room.AddEnemy(enemy);
            _pause.AddHandler(enemy);
        }

        _room.Initialize(_player, _playerView, _playerInput);
        _gameboard.HideGrid();
        _game.Initialize(_player, _pause);
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
