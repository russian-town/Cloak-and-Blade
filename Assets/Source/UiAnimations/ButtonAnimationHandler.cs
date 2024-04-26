using System;
using DG.Tweening;
using UnityEngine;

namespace Source.UiAnimations
{
    public class ButtonAnimationHandler : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private Vector3 _targetPosition;

        private Vector3 _targetScale = new Vector3(.95f, .95f, 1);
        private Vector3 _shrinkScale = new Vector3(1.05f, 1.05f, 1);
        private float _bounceDuration = .15f;
        private float _bounceInterval = .05f;
        private Vector3 _initialScale;
        private RectTransform _transform;
        private Sequence _bounceSequence;

        public event Action PoppingOut;

        public event Action Bounce;

        private void OnEnable()
        {
            _transform = GetComponent<RectTransform>();
            _initialScale = transform.localScale;
        }

        public void PopOut()
        {
            _transform.DOAnchorPos(_targetPosition, _duration, false).SetEase(Ease.Flash);
            PoppingOut?.Invoke();
        }

        public void PopBack()
            => _transform.DOAnchorPos(_initialPosition, _duration, false).SetEase(Ease.Flash);

        public void Shrink()
            => _transform.DOScale(_shrinkScale, _bounceDuration).SetEase(Ease.Flash);

        public void ShrinkBack()
            => _transform.DOScale(_initialScale, _bounceDuration).SetEase(Ease.Flash);

        public void BounceOnClick()
        {
            _bounceSequence = DOTween.Sequence();
            _bounceSequence.Append(_transform.DOScale(_targetScale, _bounceDuration).SetEase(Ease.Flash)).
                AppendInterval(_bounceInterval).Append(_transform.DOScale(_initialScale, _bounceDuration).SetEase(Ease.Flash));
            Bounce?.Invoke();
        }
    }
}
