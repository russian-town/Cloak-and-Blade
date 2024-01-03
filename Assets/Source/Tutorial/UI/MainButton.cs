using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(Button))]
public class MainButton : MonoBehaviour
{
    private LightButtonEffectHandler _effectHandler;
    private CanvasGroup _canvasGroup;
    private Button _button;

    public bool IsOpen {  get; private set; }

    public event Action<MainButton> MainButtonClicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(() => MainButtonClicked?.Invoke(this));
    }

    private void Awake()
    {
        _effectHandler = GetComponentInChildren<LightButtonEffectHandler>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(() => MainButtonClicked?.Invoke(this));
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide() 
    {
        if (_effectHandler != null)
            _effectHandler.StopLightEffect();

        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        if (_effectHandler != null)
            _effectHandler.PlayLightEffect();

        IsOpen = true;
    }
}
