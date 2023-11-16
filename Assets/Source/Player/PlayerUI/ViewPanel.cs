using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    public event Action RestartButtonClicked;
    public event Action ExitButtonClicked;

    private void OnDisable() => Dismiss();

    public virtual void Show()
    {
        _animationHandler.ScreenFadeIn();
    }

    public virtual void Hide() 
    {
        _animationHandler.ScreenFadeOut();
    } 

    public virtual void Initialize()
    {
        _restartButton.onClick.AddListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
        gameObject.SetActive(true);
    }

    protected virtual void Dismiss()
    {
        _restartButton.onClick.RemoveListener(() => RestartButtonClicked?.Invoke());
        _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());
    }
}
