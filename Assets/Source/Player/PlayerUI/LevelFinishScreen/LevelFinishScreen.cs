using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    public event UnityAction ExitButtonClicked;

    private void OnEnable() => _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());

    private void OnDisable() => _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
