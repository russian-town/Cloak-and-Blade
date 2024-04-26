using System.Collections.Generic;
using Cinemachine;
using Lean.Localization;
using Source.Enemy;
using Source.Game;
using Source.Gameboard.Cell;
using Source.Ghost.GhostSpawner;
using Source.InteractiveObjects;
using Source.InteractiveObjects.Objects.LevelExit;
using Source.LevelLoader.LoadingScreen;
using Source.Pause;
using Source.Player.PlayerInput;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using Source.Root;
using Source.Saves;
using Source.Shop;
using Source.Shop.PlayersHandler;
using Source.Sound_and_music;
using Source.Tutorial.TutorialZones;
using Source.Yandex;
using Source.Yandex.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Tutorial
{
    public class Bootstrap : MonoBehaviour, IInitializable, IDataReader
    {
        private readonly Wallet _wallet = new ();
        private readonly List<Enemy.Enemy> _enemies = new ();
        private readonly YandexAds _yandexAds = new ();
        private readonly Saver _saver = new ();

        [SerializeField] private Player.Player _player;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private InputView _inputView;
        [SerializeField] private Cell _playerSpawnCell;
        [SerializeField] private Gameboard.Gameboard _gameboard;
        [SerializeField] private CinemachineVirtualCamera _angledCamera;
        [SerializeField] private CinemachineVirtualCamera _straightCamera;
        [SerializeField] private ParticleSystem _abilityRangeTemplate;
        [SerializeField] private EnemySetter _enemySetter;
        [SerializeField] private Room.Room _room;
        [SerializeField] private Hourglass _hourglass;
        [SerializeField] private InteractiveObject[] _interactiveObjects;
        [SerializeField] private EnemyZoneDrawer _enemyZoneDrawerTemplate;
        [SerializeField] private GhostSpawner _spawner;
        [SerializeField] private Game.Game _game;
        [SerializeField] private StepCounter.StepCounter _stepCounter;
        [SerializeField] private ScoreDefiner.ScoreDefiner _scoreDefiner;
        [SerializeField] private LevelExit _levelExit;
        [SerializeField] private PlayersHandler _playersHandler;
        [SerializeField] private Audio _audio;
        [SerializeField] private FocusHandler.FocusHandler _focusHandler;
        [SerializeField] private RewardedAdHandler _rewardAdHandler;
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Battery _battery;
        [SerializeField] private LeanLocalization _localization;
        [SerializeField] private CompleteTutorialZone _completeTutorialZone;
        [SerializeField] private Button _exitButton;
        [SerializeField] private LevelLoader.LevelLoader _levelLoader;
        [SerializeField] private LevelFinisher _levelFinisher;

        private AdHandler _adHandler;
        private Pause.Pause _pause;
        private string _currentLanguage;

        private void OnEnable()
        {
            _saver.Enable();
            _levelExit.ExitOpened += OnLevelComplete;
        }

        private void OnDisable()
        {
            _saver.Disable();
            _playerView.Unsubscribe();
            _room.Unsubscribe();
            _game.Unsubscribe();
            _player.Unsubscribe();
            _levelExit.ExitOpened -= OnLevelComplete;
        }

        private void Start()
        {
            _saver.AddDataReaders(new IDataReader[] { _wallet, _completeTutorialZone, this, _levelFinisher });
            _saver.AddDataWriters(new IDataWriter[] { _wallet, _completeTutorialZone, _levelFinisher });
            _saver.AddInitializable(this);
            _saver.Initialize();
            _saver.Load();
        }

        public void Initialize()
        {
            _player = _spawner.Get(_playerSpawnCell, _player) as Player.Player;
            _player.Initialize(_playerSpawnCell, _hourglass, _room, _gameboard, _rewardAdHandler, _playerView, _battery);
            _playerView.Initialize(_player, _player.CommandExecuter, _levelFinisher, _pause);
            _room.Initialize(_player, _playerView, _hourglass);
            _inputView.Initialize();
            _rewardAdHandler.Initialize(_player, _yandexAds, _wallet, _adHandler);
            _angledCamera.Follow = _player.transform;
            _angledCamera.LookAt = _player.transform;
            _straightCamera.Follow = _player.transform;
            _straightCamera.LookAt = _player.transform;

            foreach (Cell cell in _gameboard.Cells)
                cell.View.Initialize(_abilityRangeTemplate);

            foreach (var interactiveObject in _interactiveObjects)
                interactiveObject.Initialize(_player);

#if UNITY_WEBGL && !UNITY_EDITOR
        if (string.IsNullOrEmpty(_currentLanguage) == false)
        {
            _localization.SetCurrentLanguage(_currentLanguage);
        } 
        else
        {
            switch (YandexGamesSdk.Environment.i18n.lang)
            {
                case Constants.En:
                    _localization.SetCurrentLanguage(Constants.English);
                    break;
                case Constants.Ru:
                    _localization.SetCurrentLanguage(Constants.Russian);
                    break;
                case Constants.Tr:
                    _localization.SetCurrentLanguage(Constants.Turkish);
                    break;
                default:
                    _localization.SetCurrentLanguage(Constants.English);
                    break;
            }
        }
#endif

            _pause = new Pause.Pause(new List<IPauseHandler> { _inputView, _playerView, _player, _hourglass });
            _adHandler = new AdHandler(_game, _focusHandler, _audio);
            _game.Initialize(_pause, _levelFinisher, _levelLoader, _yandexAds);
            _focusHandler.SetActiveScene(_game);
            _gameboard.HideGrid();
            _stepCounter.Initialize(_player);
            _scoreDefiner.Initialize();
            _loadingScreen.Initialize();
            _loadingScreen.StartFade(0);
        }

        public void SpawnEnemy()
        {
            EnemyZoneDrawer zoneDrawer = Instantiate(_enemyZoneDrawerTemplate, new Vector3(0, 0.1f, 0), Quaternion.identity);
            Enemy.Enemy enemy = (Enemy.Enemy)_spawner.Get(_enemySetter.Destinations[0], _enemySetter.EnemyTemplate);
            enemy.Initialize(_enemySetter.Destinations, _player, _gameboard, zoneDrawer);
            _enemies.Add(enemy);
            _room.AddEnemy(enemy);
            _pause.AddHandler(enemy);
            _player.SetTargets(_enemies);
        }

        public void RemoveEnemy()
        {
            _room.RemoveEnemies();

            if (_enemies.Count == 0)
                return;

            foreach (Enemy.Enemy enemy in _enemies)
            {
                _pause.RemoveHandler(enemy);
                enemy.Die();
            }
        }

        public void Read(PlayerData playerData)
        {
            if (playerData.IsTutorialCompleted)
                _exitButton.gameObject.SetActive(true);
            else
                _exitButton.gameObject.SetActive(false);

            _currentLanguage = playerData.CurrentLanguague;
        }

        private void OnLevelComplete()
            => _saver.Save();
    }
}
