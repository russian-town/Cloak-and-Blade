using UnityEngine;

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
    }

    public void Initialize(Player player, Pause pause)
    {
        _player = player;
        _player.Died += OnPlayerDead;
        _pause = pause;
        _playerView.PauseButtonClicked += SetPause;
        _pauseScreen.ContionueButtonClicked += Continue;
    }

    public void SetPause() 
    {
        _pause.Enable();
        _pauseScreen.Show();
        _playerView.Hide();
    }

    public void Continue()
    {
        _pause.Disable();
        _pauseScreen.Hide();
        _playerView.Show();
    }

    private void OnPlayerDead() => GameOver();

    private void GameOver() 
    {
        _playerView.Hide();
        _gameOverView.Show();
    }
}
