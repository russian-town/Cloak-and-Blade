using UnityEngine;
using UnityEngine.UI;

public class ChestTutorial : MonoBehaviour
{
    [SerializeField] private Button _keyButton;
    [SerializeField] private Canvas _playerUI;
    [SerializeField] private Canvas _chestGuide;
    [SerializeField] private Button _closeButton;

    void Start()
    {
        _chestGuide.gameObject.SetActive(false);
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
        _playerUI.gameObject.SetActive(false);
        _chestGuide.gameObject.SetActive(true);
        _keyButton.onClick.RemoveListener(() => ShowTutorialScreen());
    }

    public void Close()
    {
        _playerUI.gameObject.SetActive(true);
        _chestGuide.gameObject.SetActive(false);
    }
}
