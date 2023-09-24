using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public event Action RestartButtonClicked;
    public event Action ExitButtonClicked;

    private void OnEnable() => Initialize();

    private void OnDisable() => Dismis();

    public virtual void Show() => gameObject.SetActive(true);

    public virtual void Hide() => gameObject.SetActive(false);

    protected virtual void Initialize()
    {
        _restartButton.onClick.AddListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
    }

    protected virtual void Dismis()
    {
        _restartButton.onClick.RemoveListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());
    }
}
