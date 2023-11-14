using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonAnimationHandler : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _bounceDuration;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private Vector3 _shrinkScale;

    private float _bounceInterval = .05f;
    private Vector3 _initialScale;
    private RectTransform _transform;
    private Sequence _bounceSequence;

    private void OnEnable()
    {
        _transform = GetComponent<RectTransform>();
        _initialScale = transform.localScale;
    }

    public void PopOut() => _transform.DOAnchorPos(_targetPosition, _duration, false).SetEase(Ease.Flash);

    public void PopBack() => _transform.DOAnchorPos(_initialPosition, _duration, false).SetEase(Ease.Flash);

    public void Shrink() => _transform.DOScale(_shrinkScale, _bounceDuration).SetEase(Ease.Flash);

    public void ShrinkBack() => _transform.DOScale(_initialScale, _bounceDuration).SetEase(Ease.Flash);

    public void BounceOnClick()
    {
        _bounceSequence = DOTween.Sequence();
        _bounceSequence.Append(_transform.DOScale(_targetScale, _bounceDuration).SetEase(Ease.Flash)).
            AppendInterval(_bounceInterval).Append(_transform.DOScale(_initialScale, _bounceDuration).SetEase(Ease.Flash));
    }
}
