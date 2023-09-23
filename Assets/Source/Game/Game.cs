using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _playerUI;

    private Player _player;
    private Pause _pause;

    private void OnDisable()
    {
        _player.Died -= OnPlayerDead;
    }

    public void Initialize(Player player, Pause pause)
    {
        _player = player;
        _player.Died += OnPlayerDead;
        _pause = pause;
    }

    public void StartGame() { }

    public void PauseGame() 
    {
        _pause.Enable();
        _pauseScreen.SetActive(true);
        _playerUI.SetActive(false);
    }

    public void Continue()
    {
        _pause.Disable();
        _pauseScreen.SetActive(false);
        _playerUI.SetActive(true);
    }

    public void GameOver() { }

    private void OnPlayerDead()
    {
        GameOver();
    }
}
