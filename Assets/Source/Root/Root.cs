using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour, IInitializable
{
    [SerializeField] private List<Player> _playerTemplates = new List<Player>();
    [SerializeField] private Player _defaultPlayerTemplate;
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
    [SerializeField] private Hourglass _hourglass;
    [SerializeField] private InteractiveObject[] _interactiveObjects;
    [SerializeField] private EnemyZoneDrawer _enemyZoneDrawerTemplate;
    [SerializeField] private GhostSpawner _spawner;
    [SerializeField] private Game _game;
    [SerializeField] private StepCounter _stepCounter;
    [SerializeField] private ScoreDefiner _scoreDefiner;
    [SerializeField] private LevelExit _levelExit;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private Saver _saver;
    [SerializeField] private List<EffectChangeHanldler> _effectChangeHanldlers = new List<EffectChangeHanldler>();

    private Player _player;
    private Pause _pause;
    private List<Enemy> _enemies = new List<Enemy>();

    private void OnDisable()
    {
        _playerView.Unsubscribe();
        _room.Unsubscribe();
        _game.Unsubscribe();
        _player.Unsubscribe();
    }

    private void Start()
    {
        _saver.AddDataReaders(new IDataReader[] {_playersHandler});
        _saver.AddDataWriters(new IDataWriter[] { _playersHandler });
        _saver.AddInitializable(this);
        _saver.Initialize();
        _saver.Load();
    }

    public void Initialize()
    {
        if (_playerTemplates.Contains(_playersHandler.CurrentPlayer))
        {
            int index = _playerTemplates.IndexOf(_playersHandler.CurrentPlayer);
            _player = (Player)_spawner.Get(_playerSpawnCell, _playerTemplates[index]);
        }
        else
        {
            _player = (Player)_spawner.Get(_playerSpawnCell, _defaultPlayerTemplate);
        }

        _hourglass.Initialaze();
        _player.Initialize(_playerSpawnCell, _hourglass, _room, _playerView, _gameboard, _playerInput);
        _playerInput.Initialize(_camera, _gameboard, _player);
        _playerView.Initialize(_player);
        _room.Initialize(_player, _playerView, _playerInput);
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
            _enemies.Add(enemy);
            _room.AddEnemy(enemy);
            _pause.AddHandler(enemy);
        }

        _player.SetTargets(_enemies);
        _game.Initialize(_player, _pause, _levelExit);
        _gameboard.HideGrid();
        _stepCounter.Initialize(_player);
        _scoreDefiner.Initialize();
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
