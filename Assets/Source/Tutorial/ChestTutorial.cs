using UnityEngine;
using UnityEngine.UI;

public class ChestTutorial : MonoBehaviour
{
    [SerializeField] private Button _keyButton;
    [SerializeField] private PlayerView _playerUI;
    [SerializeField] private ScreenAnimationHandler _chestGuide;
    [SerializeField] private Button _closeButton;

    void Start()
    {
        _keyButton.onClick.AddListener(() => ShowTutorialScreen());
        _closeButton.onClick.AddListener(() => Close());
    }

    private void OnDisable()
    {
        _keyButton.onClick.RemoveListener(() => ShowTutorialScreen());
        _closeButton.onClick.RemoveListener(() => Close());
    }

    public void ShowTutorialScreen()
    {
        _playerUI.Hide();
        _chestGuide.ScreenFadeIn();
        _keyButton.onClick.RemoveListener(() => ShowTutorialScreen());
    }

    public void Close()
    {
        _playerUI.Show();
        _chestGuide.ScreenFadeOut();
    }
}
