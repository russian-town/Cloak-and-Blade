using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private PauseView _pauseScreen;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameOverView _gameOverView;

    private Player _player;
    private Pause _pause;

    public void Unsubscribe()
    {
        _player.Died -= OnPlayerDead;
        _playerView.PauseButtonClicked -= SetPause;
        _pauseScreen.ContionueButtonClicked -= Continue;
        _pauseScreen.RestartButtonClicked -= Restart;
        _pauseScreen.ExitButtonClicked -= Exit;
        _gameOverView.RestartButtonClicked -= Restart;
        _gameOverView.ExitButtonClicked -= Exit;
    }

    public void Initialize(Player player, Pause pause)
    {
        _player = player;
        _player.Died += OnPlayerDead;
        _pause = pause;
        _pauseScreen.Initialize();
        _gameOverView.Initialize();
        _playerView.PauseButtonClicked += SetPause;
        _pauseScreen.ContionueButtonClicked += Continue;
        _pauseScreen.RestartButtonClicked += Restart;
        _pauseScreen.ExitButtonClicked += Exit;
        _gameOverView.RestartButtonClicked += Restart;
        _gameOverView.ExitButtonClicked += Exit;
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

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void Exit() => SceneManager.LoadScene(Constants.MainMenu);

    private void OnPlayerDead() => GameOver();

    private void GameOver() 
    {
        _playerView.Hide();
        _gameOverView.Show();
    }
}
