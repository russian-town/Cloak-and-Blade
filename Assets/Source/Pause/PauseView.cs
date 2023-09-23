using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public event Action ContionueButtonClicked;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(() => ContionueButtonClicked?.Invoke());
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(() => ContionueButtonClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
