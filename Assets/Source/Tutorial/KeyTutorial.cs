using UnityEngine;
using UnityEngine.UI;

public class KeyTutorial : MonoBehaviour
{
    [SerializeField] private Button _chestButton;
    [SerializeField] private Canvas _playerUI;
    [SerializeField] private Canvas _keyGuide;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Key _key;

    void Start()
    {
        _keyGuide.gameObject.SetActive(false);
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

        _playerUI.gameObject.SetActive(false);
        _keyGuide.gameObject.SetActive(true);
        _chestButton.onClick.RemoveListener(() => ShowTutorialScreen());
    }

    public void Close()
    {
        _playerUI.gameObject.SetActive(true);
        _keyGuide.gameObject.SetActive(false);
    }
}
