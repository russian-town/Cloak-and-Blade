using DG.Tweening;
using UnityEngine;

public class ChainDOTanimation : MonoBehaviour
{
    private readonly float _outOfScreenPosition = -300;
    private readonly float _shakeLength = 2f;
    private readonly float _fadeLength = 2f;
    private readonly float _shakeStrength = 3;
    private readonly int _shakeVibrato = 8;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _animationLength;

    public void PlayFallAnimation()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _rectTransform.DOAnchorPos(new Vector2(0f, _outOfScreenPosition), _animationLength, false).SetEase(Ease.InCirc);
        _canvasGroup.DOFade(0, _fadeLength);
    }

    public void PlayShakeAnimation()
    {
        _rectTransform.DOShakeAnchorPos(_shakeLength, _shakeStrength, _shakeVibrato, 0);
    }
}
