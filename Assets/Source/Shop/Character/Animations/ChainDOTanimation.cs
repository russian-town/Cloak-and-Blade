using DG.Tweening;
using UnityEngine;

public class ChainDOTanimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _animationLength;

    private float _outOfScreenPosition = -300;
    private float _shakeLength = 2f;
    private float _fadeLength = 2f;

    public void PlayFallAnimation()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _rectTransform.DOAnchorPos(new Vector2(0f, _outOfScreenPosition), _animationLength, false).SetEase(Ease.InCirc);
        _canvasGroup.DOFade(0, _fadeLength);
    }

    public void PlayShakeAnimation()
    {
        _rectTransform.DOShakeAnchorPos(_shakeLength, 3, 8, 0);
    }
}
