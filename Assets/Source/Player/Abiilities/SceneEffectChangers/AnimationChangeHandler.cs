using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationChangeHandler : MonoBehaviour
{
    private Animator _animator;
    private Coroutine _changeSpeedOverTime;

    public float InitialSpeed { get; private set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        InitialSpeed = _animator.speed;
    }

    public void ChangeSpeed(float value, float duration)
    {
        if (_changeSpeedOverTime != null)
            return;

        _changeSpeedOverTime = StartCoroutine(ChangeSpeedOverTime(value, duration));
    }

    private IEnumerator ChangeSpeedOverTime(float value, float duration)
    {
        while (_animator.speed != value)
        {
            _animator.speed = Mathf.MoveTowards(_animator.speed, value, duration * Time.deltaTime);
            yield return null;
        }

        _changeSpeedOverTime = null;
    }
}
