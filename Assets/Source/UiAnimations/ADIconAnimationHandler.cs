using UnityEngine;
using DG.Tweening;

public class ADIconAnimationHandler : MonoBehaviour
{
    [SerializeField] private float _animationDuration;

    private Sequence _rotateDancingSequence;
    private Vector3 _initialRotation;
    private Vector3 _rightRotation = new Vector3(0, 0, -15);
    private Vector3 _leftRotation = new Vector3(0, 0, 20);

    private void Start()
    {
        _initialRotation = Vector3.zero;
    }

    private void OnEnable()
    {
        PlayDancingAnimation();
    }

    private void PlayDancingAnimation()
    {
        transform.DOShakeScale(_animationDuration, .15f, 0, 0, false).SetLoops(-1);
        _rotateDancingSequence = DOTween.Sequence();
        _rotateDancingSequence.
            Append(transform.DORotate(_rightRotation, _animationDuration)).
            Append(transform.DORotate(_leftRotation, _animationDuration)).
            Append(transform.DORotate(_initialRotation, _animationDuration)).SetLoops(-1);
    }
}
