using Cinemachine;
using Lean.Localization;
using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

public class Root : MonoBehaviour, IInitializable
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private InputView _inputView;
    [SerializeField] private Cell _playerSpawnCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private ParticleSystem _abilityRangeTemplate;
    [SerializeField] private EnemySetter[] _enemySetters;
    [SerializeField] private Room _room;
    [SerializeField] private Hourglass _hourglass;
    [SerializeField] private InteractiveObject[] _interactiveObjects;
    [SerializeField] private EnemyZoneDrawer _enemyZoneDrawerTemplate;
    [SerializeField] private GhostSpawner _spawner;
    [SerializeField] private Game _game;
    [SerializeField] private StepCounter _stepCounter;
    [SerializeField] private ScoreDefiner _scoreDefiner;
    [SerializeField] private LevelExit _levelExit;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private Audio _audio;
    [SerializeField] private FocusHandler _focusHandler;
    [SerializeField] private RewardedAdHandler _rewardAdHandler;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private Battery _battery;
    [SerializeField] private LeanLocalization _localization;

    private readonly Saver _saver = new Saver();
    private readonly Wallet _wallet = new Wallet();
    private readonly List<Enemy> _enemies = new List<Enemy>();
    private readonly YandexAds _yandexAds = new YandexAds();

    private AdHandler _adHandler;
    private Player _player;
    private Pause _pause;

    public Saver Saver => _saver;

    public void OnEnable() => _saver.Enable();

    private void OnDisable()
    {
        _saver.Save();
        _saver.Disable();
        _playerView.Unsubscribe();
        _room.Unsubscribe();
        _game.Unsubscribe();
        _player.Unsubscribe();
        _yandexAds.OpenInterstitialCallback -= OnAdOpenCallback;
        _yandexAds.CloseInterstitialCallback -= OnInterstetialClose;
        _yandexAds.OpenCallback -= OnAdOpenCallback;
        _yandexAds.CloseCallback -= OnAdRewardedCloseCallback;
    }

    private void Start()
    {
        _saver.AddDataReaders(new IDataReader[] { _playersHandler, _wallet, _audio, _game });
        _saver.AddDataWriters(new IDataWriter[] { _playersHandler, _wallet, _game });
        _saver.AddInitializable(this);
        _saver.AddInitializable(_wallet);
        _saver.Initialize();
        _saver.Load();
    }

    public void Initialize()
    {
        _yandexAds.OpenInterstitialCallback += OnAdOpenCallback;
        _yandexAds.CloseInterstitialCallback += OnInterstetialClose;
        _yandexAds.OpenCallback += OnAdOpenCallback;
        _yandexAds.CloseCallback += OnAdRewardedCloseCallback;
        _yandexAds.ShowInterstitial();
        _player = _playersHandler.GetPlayer(_spawner, _playerSpawnCell);
        _player.Initialize(_playerSpawnCell, _hourglass, _room, _gameboard, _rewardAdHandler, _playerView, _battery);
        _playerView.Initialize(_player, _player.CommandExecuter);
        _room.Initialize(_player, _playerView, _hourglass);
        _inputView.Initialize();
        _rewardAdHandler.Initialize(_player, _yandexAds);
        _walletView.Initialize(_wallet);
        _angledCamera.Follow = _player.transform;
        _angledCamera.LookAt = _player.transform;
        _straightCamera.Follow = _player.transform;
        _straightCamera.LookAt = _player.transform;

        foreach (Cell cell in _gameboard.Cells)
            cell.View.Initialize(_abilityRangeTemplate);

        foreach (var interactiveObject in _interactiveObjects)
            interactiveObject.Initialize(_player);

#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.Environment.i18n.lang == "en")
            _localization.SetCurrentLanguage(Constants.English);

        if (YandexGamesSdk.Environment.i18n.lang == "ru")
            _localization.SetCurrentLanguage(Constants.Russian);

        if (YandexGamesSdk.Environment.i18n.lang == "tr")
            _localization.SetCurrentLanguage(Constants.Turkish);
#endif

        _pause = new Pause(new List<IPauseHandler> { _inputView, _playerView, _player, _hourglass });

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
        _adHandler = new AdHandler(_game, _focusHandler, _audio);
        _game.Initialize(_player, _pause, _levelExit, _wallet, _adHandler);
        _focusHandler.SetActiveScene(_game);
        _gameboard.HideGrid();
        _stepCounter.Initialize(_player);
        _scoreDefiner.Initialize();
        _loadingScreen.Initialize();
        _loadingScreen.StartFade(0);
    }

    private void OnAdOpenCallback()
    {
        _focusHandler.enabled = false;
        _game.SetPause();
        _audio.Mute();
    }

    private void OnAdRewardedCloseCallback()
    {
        _audio.UnMute();
        _focusHandler.enabled = true;
    }

    private void OnInterstetialClose(bool obj)
    {
        _audio.UnMute();
        _focusHandler.enabled = true;
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
