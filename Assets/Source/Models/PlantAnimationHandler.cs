using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlantAnimationHandler : MonoBehaviour
{
    [SerializeField] private int _maxStartDelay = 2000;
    [SerializeField] private float _period;
    [SerializeField] private float _delay;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _axis;
    [SerializeField] private float _maxAngle;
    [SerializeField] private bool _initializeOnStart;
    [SerializeField] private Ease _ease;
    [SerializeField] private Ease _easeBack;

    private Vector3 _initialRotation;
    private bool _isInitialized;
    private Sequence sequence;
    private System.Random _random;

    private IEnumerator Start()
    {
        if (_initializeOnStart)
            yield return Initialize();
    }

    private IEnumerator Initialize()
    {
        _random = new System.Random();
        _initialRotation = _targetTransform.localRotation.eulerAngles;
        yield return StartLocalTween();
        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_isInitialized == false)
            return;

        sequence?.Kill();
    }

    private IEnumerator StartLocalTween()
    {
        int startDelay = _random.Next(_maxStartDelay);
        yield return new WaitForSeconds(startDelay / 1000f);
        sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.Append(_targetTransform
            .DOLocalRotate(_axis * -_maxAngle, _period / 4)
            .SetEase(_ease));
        sequence.AppendInterval(_delay);

        sequence.Append(_targetTransform
            .DOLocalRotate(_initialRotation, _period / 4)
            .SetEase(_easeBack));

        sequence.Append(_targetTransform
            .DOLocalRotate(_axis * _maxAngle, _period / 4)
            .SetEase(_ease));
        sequence.AppendInterval(_delay);

        sequence.Append(_targetTransform
            .DOLocalRotate(_initialRotation, _period / 4)
            .SetEase(_easeBack));

        sequence.SetLoops(-1, LoopType.Restart);
        sequence.Play();
    }
}
