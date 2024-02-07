using UnityEngine;

[RequireComponent(typeof(MainMenuVeiw))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelsViewScroll _levelsViewScroll;
    [SerializeField] private LevelLoader _levelsLoader;

    private MainMenuVeiw _mainMenuVeiw;

    private void OnDisable() => _mainMenuVeiw.PlayButtonClicked -= StartGame;

    public void Initialize()
    {
        _mainMenuVeiw = GetComponent<MainMenuVeiw>();
        _mainMenuVeiw.PlayButtonClicked += StartGame;
        _levelsViewScroll.Hide();
    }

    private void StartGame() => _levelsViewScroll.Show();
}
