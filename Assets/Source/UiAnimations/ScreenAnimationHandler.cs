using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;

public class ScreenAnimationHandler : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private bool _isDescriptionScreen;

    private float _outOfScreenPosition = -1200;
    private float _descriptionOutOfScreen = 1200;
    private Vector3 _initialPosition;

    public event Action ScreenEnabled;

    public bool IsEnabled { get; private set; }

    private void Awake()
    {
        _initialPosition = _rectTransform.localPosition;
    }

    public void ScreenFadeIn()
    {
        IsEnabled = true;
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        if (!_isDescriptionScreen)
            _rectTransform.transform.localPosition = new Vector2(_initialPosition.x, _outOfScreenPosition);
        else
            _rectTransform.transform.localPosition = new Vector2(_descriptionOutOfScreen, _initialPosition.y);

        _rectTransform.DOAnchorPos(_initialPosition, _fadeDuration, false).SetEase(Ease.OutQuint);
        _canvasGroup.DOFade(1, _fadeDuration);
        ScreenEnabled?.Invoke();
    }

    public void ScreenFadeOut()
    {
        IsEnabled = false;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        if (!_isDescriptionScreen)
            _rectTransform.DOAnchorPos(new Vector2(_initialPosition.x, _outOfScreenPosition), _fadeDuration, false).SetEase(Ease.InSine);
        else
            _rectTransform.DOAnchorPos(new Vector2(_descriptionOutOfScreen, _initialPosition.y), _fadeDuration, false).SetEase(Ease.InSine);

        _canvasGroup.DOFade(0, _fadeDuration);
    }
}
