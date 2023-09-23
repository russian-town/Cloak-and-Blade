using UnityEngine;

public class Game : MonoBehaviour
{
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
    }

    public void Continue()
    {
        _pause.Disable();
    }

    public void GameOver() { }

    private void OnPlayerDead()
    {
        GameOver();
    }
}
