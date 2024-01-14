using UnityEngine;
using DG.Tweening;

public class AnimalPropMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _rotationDuration;

    private Sequence _movingSequence;

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        _movingSequence = DOTween.Sequence();
        _movingSequence
            .Append(transform.DOLookAt(_secondPoint.position, _rotationDuration).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(_secondPoint.position, _moveDuration).SetEase(Ease.Linear))
            .Append(transform.DOLookAt(_firstPoint.position, _rotationDuration).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(_firstPoint.position, _moveDuration).SetEase(Ease.Linear))
            .SetLoops(-1);
    }
}
