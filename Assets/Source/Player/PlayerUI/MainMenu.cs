using UnityEngine;

[RequireComponent(typeof(MainMenuVeiw))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;

    private MainMenuVeiw _mainMenuVeiw;

    private void Awake()
    {
        _mainMenuVeiw = GetComponent<MainMenuVeiw>();
    }

    private void OnEnable()
    {
        _mainMenuVeiw.PlayButtonClicked += OnPlayButtonClicked;
    }

    private void OnDisable()
    {
        _mainMenuVeiw.PlayButtonClicked -= OnPlayButtonClicked;
    }

    private void OnPlayButtonClicked() => StartGame();

    private void StartGame()
    {
        _levelLoader.TryLoadFirstLevel();
    }
}
