using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveAlongSpline))]
public class SplineChangeHandler : MonoBehaviour
{
    private MoveAlongSpline _splineAnimate;
    private Coroutine _changeSpeedOverTime;

    public float InitialSpeed { get; private set; }

    private void Start()
    {
        _splineAnimate = GetComponent<MoveAlongSpline>();
        InitialSpeed = _splineAnimate.Speed;
    }

    public void ChangeSpeed(float value, float duration)
    {
        if (_changeSpeedOverTime != null)
            return;

        _changeSpeedOverTime = StartCoroutine(ChangeSpeedOverTime(value, duration));
    }

    private IEnumerator ChangeSpeedOverTime(float value, float duration)
    {
        while (_splineAnimate.Speed != value)
        {
            _splineAnimate.ChangeSlineSpeed(value, duration, InitialSpeed);
            yield return null;
        }

        _changeSpeedOverTime = null;
    }
}
