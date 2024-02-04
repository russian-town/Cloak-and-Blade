using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour, IDataWriter, IDataReader, IActiveScene
{
    [SerializeField] private PauseView _pauseScreen;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private StepCounter _stepCounter;
    [SerializeField] private ScoreDefiner _scoreDefiner;
    [SerializeField] private LevelFinishScreen _levelFinishScreen;
    [SerializeField] private LevelsHandler _levelsHandler;

    private Wallet _wallet;
    private Player _player;
    private Pause _pause;
    private YandexAds _yandexAds;
    private ILevelFinisher _levelFinisher;
    private bool _levelPassed;
    private bool _gameOver;
    private AdHandler _adHandler;
    private List<string> _finishedLevelNames = new List<string>();
    private List<int> _finishedLevelStarsCount = new List<int>();

    public bool IsInitialize { get; private set; }

    public void Unsubscribe()
    {
        _player.Died -= OnPlayerDead;
        _playerView.PauseButtonClicked -= SetPause;
        _pauseScreen.ContionueButtonClicked -= Continue;
        _pauseScreen.RestartButtonClicked -= Restart;
        _pauseScreen.ExitButtonClicked -= Exit;
        _gameOverView.RestartButtonClicked -= Restart;
        _gameOverView.ExitButtonClicked -= Exit;
        _levelFinishScreen.ExitButtonClicked -= Exit;
        _levelFinishScreen.NextLevelButtonClicked -= OnNextLevelButtonClicked;
        _levelFinisher.LevelPassed -= OnLevelPassed;
        _yandexAds.OpenCallback -= OnRewardedOpenClallback;
        _yandexAds.RewardedCallback -= OnRewardedClallback;
        _yandexAds.CloseCallback -= OnRewardedCloseClallback;
    }

    public void Initialize(Player player, Pause pause, ILevelFinisher levelFinisher, Wallet wallet, AdHandler adHandler)
    {
        _wallet = wallet;
        _player = player;
        _yandexAds = new YandexAds();
        _adHandler = adHandler;
        _yandexAds.OpenCallback += OnRewardedOpenClallback;
        _yandexAds.RewardedCallback += OnRewardedClallback;
        _yandexAds.CloseCallback += OnRewardedCloseClallback;
        _player.Died += OnPlayerDead;
        _pause = pause;
        _pauseScreen.Initialize();
        _gameOverView.Initialize();
        _levelFinisher = levelFinisher;
        _playerView.PauseButtonClicked += SetPause;
        _pauseScreen.ContionueButtonClicked += Continue;
        _pauseScreen.RestartButtonClicked += Restart;
        _pauseScreen.ExitButtonClicked += Exit;
        _gameOverView.RestartButtonClicked += Restart;
        _gameOverView.ExitButtonClicked += Exit;
        _levelFinisher.LevelPassed += OnLevelPassed;
        _levelFinishScreen.ExitButtonClicked += Exit;
        _levelFinishScreen.NextLevelButtonClicked += OnNextLevelButtonClicked;
        _levelFinishScreen.Hide();
        _levelFinishScreen.Initialize(_yandexAds);
        IsInitialize = true;
    }

    public void SetPause()
    {
        if (!_levelPassed && !_gameOver)
            _pauseScreen.Show();

        _playerView.Hide();
        _pause.Enable();
    }

    public void Continue()
    {
        if (!_levelPassed && !_gameOver)
        {
            _pauseScreen.Hide();
            _playerView.Show();
        }

        _pause.Disable();
    }

    public void Write(PlayerData playerData)
    {
        playerData.FinishedLevelNames = _finishedLevelNames;
        playerData.FinishedLevelsStarsCount = _finishedLevelStarsCount;
    }

    public void Read(PlayerData playerData)
    {
        _finishedLevelNames = playerData.FinishedLevelNames;
        _finishedLevelStarsCount = playerData.FinishedLevelsStarsCount;
    }

    private void AddStarsOnReward()
    {
        _wallet.AddStars(_scoreDefiner.StarsCount);
    }

    private void OnLevelPassed()
    {
        _levelPassed = true;
        _playerView.Hide();
        _levelFinishScreen.Show();
        _scoreDefiner.RecieveStars(_stepCounter.CurrentStepCount);
        _wallet.AddStars(_scoreDefiner.StarsCount);

        if (_finishedLevelNames.Contains(SceneManager.GetActiveScene().name))
        {
            int index = _finishedLevelNames.IndexOf(SceneManager.GetActiveScene().name);

            if (_scoreDefiner.StarsCount > _finishedLevelStarsCount[index])
                _finishedLevelStarsCount[index] = _scoreDefiner.StarsCount;

            return;
        }

        _finishedLevelNames.Add(SceneManager.GetActiveScene().name);
        _finishedLevelStarsCount.Add(_scoreDefiner.StarsCount);
    }

    private void OnRewardedOpenClallback() => _adHandler.OpenAd();
    
    private void OnRewardedClallback()
    {
        AddStarsOnReward();
        _levelFinishScreen.Unsubscribe();
    }

    private void OnRewardedCloseClallback() => _adHandler.CloseAd();

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void Exit() => SceneManager.LoadScene(Constants.MainMenu);

    private void OnPlayerDead() => GameOver();

    private void OnNextLevelButtonClicked() => SceneManager.LoadScene(_levelsHandler.GetNextLevel().Name);

    private void GameOver() 
    {
        _gameOver = true;
        _playerView.Hide();
        _gameOverView.Show();
    }
}
