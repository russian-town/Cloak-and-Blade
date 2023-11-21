using UnityEngine;
using UnityEngine.UI;

public class KeyTutorial : MonoBehaviour
{
    [SerializeField] private Button _chestButton;
    [SerializeField] private PlayerView _playerUI;
    [SerializeField] private ScreenAnimationHandler _keyGuide;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Key _key;

    void Start()
    {
        _chestButton.onClick.AddListener(() => ShowTutorialScreen());
        _closeButton.onClick.AddListener(() => Close());
    }

    private void OnDisable()
    {
        _chestButton.onClick.RemoveListener(() => ShowTutorialScreen());
        _closeButton.onClick.RemoveListener(() => Close());
    }

    public void ShowTutorialScreen()
    {
        if (!_key.isActiveAndEnabled)
            return;

        _playerUI.Hide();
        _keyGuide.ScreenFadeIn();
        _chestButton.onClick.RemoveListener(() => ShowTutorialScreen());
    }

    public void Close()
    {
        _playerUI.Show();
        _keyGuide.ScreenFadeOut();
    }
}
