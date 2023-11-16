using UnityEngine;
using DG.Tweening;

public class ScreenAnimationHandler : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;

    private float _outOfScreenPostition = -1000;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = _rectTransform.localPosition;
    }

    public void ScreenFadeIn()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _rectTransform.transform.localPosition = new Vector3(0, _outOfScreenPostition, 0);
        _rectTransform.DOAnchorPos(_initialPosition, _fadeDuration, false).SetEase(Ease.OutElastic);
        _canvasGroup.DOFade(1, _fadeDuration);
    }

    public void ScreenFadeOut()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _rectTransform.transform.localPosition = new Vector3(0, 0, 0);
        _rectTransform.DOAnchorPos(new Vector2(0f, _outOfScreenPostition), _fadeDuration, false).SetEase(Ease.InOutQuint);
        _canvasGroup.DOFade(0, _fadeDuration);
    }
}
