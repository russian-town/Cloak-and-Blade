using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MainMenuVeiw))]
public class MainMenu : MonoBehaviour
{
    private MainMenuVeiw _mainMenuVeiw;

    private void Awake()
    {
        _mainMenuVeiw = GetComponent<MainMenuVeiw>();
    }

    private void OnEnable()
    {
        _mainMenuVeiw.PlayButtonClicked += StartGame;
    }

    private void OnDisable()
    {
        _mainMenuVeiw.PlayButtonClicked -= StartGame;
    }

    private void StartGame() => SceneManager.LoadScene("Tutorial");
}
