using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public event Action ContionueButtonClicked;
    public event Action RestartButtonClicked;
    public event Action ExitButtonClicked;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(() => ContionueButtonClicked?.Invoke());
        _restartButton.onClick.AddListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(() => ContionueButtonClicked?.Invoke());
        _restartButton.onClick.RemoveListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
