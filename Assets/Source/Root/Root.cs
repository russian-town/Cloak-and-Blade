using Cinemachine;
using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Player _playerTemplate;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private Cell _playerSpawnCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private ParticleSystem _mouseOverCell;
    [SerializeField] private ParticleSystem _enemySightEffectTemplate;
    [SerializeField] private ParticleSystem _abilityRangeTemplate;
    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private EnemySetter[] _enemySetters;
    [SerializeField] private Room _room;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private AnimationClip _hourglassAnimation;
    [SerializeField] private Animator _hourglassAnimator;
    [SerializeField] private CanvasGroup _hourglass;
    [SerializeField] private InteractiveObject[] _interactiveObjects;

    private Player _player;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = _playerSpawner.Get(_playerSpawnCell, _playerTemplate);
        _player.Initialize(_playerSpawnCell, _hourglassAnimation, _hourglassAnimator, _hourglass);
        _playerInput.Initialize(_camera, _gameboard, _mouseOverCell, _player);
        _playerView.Initialize(_player);
        _angledCamera.Follow = _player.transform;
        _angledCamera.LookAt = _player.transform;
        _straightCamera.Follow = _player.transform;
        _straightCamera.LookAt = _player.transform;

        foreach (Cell cell in _gameboard.Cells)
            cell.View.Initialize(_enemySightEffectTemplate, _abilityRangeTemplate);

        foreach (var interactiveObject in _interactiveObjects)
            interactiveObject.Initialize(_player);

        foreach (var setter in _enemySetters)
        {
            Enemy enemy = _enemySpawner.Get(setter.EnemyTemplate, setter.Destinations[0]);
            enemy.Initialize(setter.Destinations, _player, _gameboard, _musicPlayer);
            _room.AddEnemy(enemy);
        }

        _room.Initialize(_player, _playerView);
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
