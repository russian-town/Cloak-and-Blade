using UnityEngine;

public class Game : MonoBehaviour, IActiveScene
{
    [SerializeField] private PauseView _pauseScreen;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private StepCounter _stepCounter;
    [SerializeField] private LevelFinishScreen _levelFinishScreen;
    [SerializeField] private LevelsHandler _levelsHandler;

    private Pause _pause;
    private ILevelFinisher _levelFinisher;
    private LevelLoader _levelLoader;

    public void Unsubscribe()
    {
        _playerView.PauseButtonClicked -= SetPause;
        _pauseScreen.ContionueButtonClicked -= Continue;
        _pauseScreen.RestartButtonClicked -= Restart;
        _pauseScreen.ExitButtonClicked -= Exit;
        _gameOverView.RestartButtonClicked -= Restart;
        _gameOverView.ExitButtonClicked -= Exit;
        _levelFinishScreen.ExitButtonClicked -= Exit;
        _levelFinishScreen.NextLevelButtonClicked -= OnNextLevelButtonClicked;
    }

    public void Initialize(
        Pause pause,
        ILevelFinisher levelFinisher,
        LevelLoader levelLoader,
        YandexAds yandexAds)
    {
        _pause = pause;
        _pauseScreen.Subscribe();
        _gameOverView.Subscribe();
        _levelFinisher = levelFinisher;
        _pauseScreen.Initialize(_pause);
        _playerView.PauseButtonClicked += SetPause;
        _pauseScreen.ContionueButtonClicked += Continue;
        _pauseScreen.RestartButtonClicked += Restart;
        _pauseScreen.ExitButtonClicked += Exit;
        _gameOverView.RestartButtonClicked += Restart;
        _gameOverView.ExitButtonClicked += Exit;
        _levelFinishScreen.ExitButtonClicked += Exit;
        _levelFinishScreen.NextLevelButtonClicked += OnNextLevelButtonClicked;
        _levelFinishScreen.Initialize(yandexAds, _levelFinisher);
        _levelLoader = levelLoader;
    }

    public void SetPause()
        => _pause.Disable();

    public void Continue()
        => _pause.Disable();

    private void Restart()
        => _levelLoader.RestartLevel();

    private void Exit()
        => _levelLoader.BackToMainMenu();

    private void OnNextLevelButtonClicked()
        => _levelLoader.LoadNextLevel();
}
