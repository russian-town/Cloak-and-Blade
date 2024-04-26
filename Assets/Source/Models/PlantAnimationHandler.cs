using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlantAnimationHandler : MonoBehaviour
{
    private readonly int _moveRatio = 4;
    private readonly int _numberOfLoops = -1;
    private readonly float _delayRatio = 1000f;

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
    private Sequence _sequence;
    private System.Random _random;
    private WaitForSeconds _startDelayWaitForSeconds;

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
        int startDelay = _random.Next(_maxStartDelay);
        _startDelayWaitForSeconds = new WaitForSeconds(startDelay / _delayRatio);
        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_isInitialized == false)
            return;

        _sequence?.Kill();
    }

    private IEnumerator StartLocalTween()
    {
        yield return _startDelayWaitForSeconds;
        _sequence = DOTween.Sequence();
        _sequence.Pause();

        _sequence.Append(_targetTransform
            .DOLocalRotate(_axis * -_maxAngle, _period / _moveRatio)
            .SetEase(_ease));
        _sequence.AppendInterval(_delay);

        _sequence.Append(_targetTransform
            .DOLocalRotate(_initialRotation, _period / _moveRatio)
            .SetEase(_easeBack));

        _sequence.Append(_targetTransform
            .DOLocalRotate(_axis * _maxAngle, _period / _moveRatio)
            .SetEase(_ease));
        _sequence.AppendInterval(_delay);

        _sequence.Append(_targetTransform
            .DOLocalRotate(_initialRotation, _period / _moveRatio)
            .SetEase(_easeBack));

        _sequence.SetLoops(_numberOfLoops, LoopType.Restart);
        _sequence.Play();
    }
}
