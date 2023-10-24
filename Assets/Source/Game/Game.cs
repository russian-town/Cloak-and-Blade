using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private PauseView _pauseScreen;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private StepCounter _stepCounter;
    [SerializeField] private ScoreDefiner _scoreDefiner;
    [SerializeField] private LevelFinishScreen _finishLevelScreen;

    private Wallet _wallet;
    private Player _player;
    private Pause _pause;
    private ILevelFinisher _levelFinisher;

    public void Unsubscribe()
    {
        _player.Died -= OnPlayerDead;
        _playerView.PauseButtonClicked -= SetPause;
        _pauseScreen.ContionueButtonClicked -= Continue;
        _pauseScreen.RestartButtonClicked -= Restart;
        _pauseScreen.ExitButtonClicked -= Exit;
        _gameOverView.RestartButtonClicked -= Restart;
        _gameOverView.ExitButtonClicked -= Exit;
        _finishLevelScreen.ExitButtonClicked -= Exit;
        _levelFinisher.LevelPassed -= OnLevelPassed;
    }

    public void Initialize(Player player, Pause pause, ILevelFinisher levelFinisher, Wallet wallet)
    {
        _wallet = wallet;
        _player = player;
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
        _finishLevelScreen.ExitButtonClicked += Exit;
        _finishLevelScreen.Hide();
    }

    public void SetPause()
    {
        _pauseScreen.Show();
        _playerView.Hide();
        _pause.Enable();
    }

    private void Continue()
    {
        _pauseScreen.Hide();
        _playerView.Show();
        _pause.Disable();
    }

    private void OnLevelPassed()
    {
        _playerView.Hide();
        _finishLevelScreen.Show();
        _scoreDefiner.AccrueStars(_stepCounter.CurrentStepCount);
        _wallet.AddStars(_scoreDefiner.StarsCount);
    }

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void Exit() => SceneManager.LoadScene(Constants.MainMenu);

    private void OnPlayerDead() => GameOver();

    private void GameOver() 
    {
        _playerView.Hide();
        _gameOverView.Show();
    }
}
