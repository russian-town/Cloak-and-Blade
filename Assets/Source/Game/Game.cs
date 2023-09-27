using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private PauseView _pauseScreen;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private GameOverView _gameOverView;

    private Player _player;
    private Pause _pause;

    private void OnDisable()
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
        _playerView.PauseButtonClicked += SetPause;
        _pauseScreen.ContionueButtonClicked += Continue;
        _pauseScreen.RestartButtonClicked += Restart;
        _pauseScreen.ExitButtonClicked += Exit;
        _gameOverView.RestartButtonClicked += Restart;
        _gameOverView.ExitButtonClicked += Exit;
    }

    public void SetPause()
    {
        _pause.Enable();
        _pauseScreen.Show();
        _playerView.Hide();
    }

    private void Continue()
    {
        _pause.Disable();
        _pauseScreen.Hide();
        _playerView.Show();
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
