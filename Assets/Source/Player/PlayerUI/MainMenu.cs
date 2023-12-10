using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MainMenuVeiw))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelsViewScroll _levelsViewScroll;
    [SerializeField] private LevelLoader _levelsLoader;

    private MainMenuVeiw _mainMenuVeiw;

    private void Awake()
    {
        _mainMenuVeiw = GetComponent<MainMenuVeiw>();
        _levelsViewScroll.Hide();
    }

    private void OnEnable()
    {
        _mainMenuVeiw.PlayButtonClicked += StartGame;
    }

    private void OnDisable()
    {
        _mainMenuVeiw.PlayButtonClicked -= StartGame;
    }

    private void StartGame()
    {
        if (_levelsLoader.TryTutorialLoad() == false)
            _levelsViewScroll.Show();
    }
}
