using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class Hourglass : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _fadeInSpeed;
    [SerializeField] private AnimationClip _hourglassClip;

    private CanvasGroup _canvasGroup;
    private Animator _animator;
    private float _pauseSpeed = 1;

    public float AnimationLength => _hourglassClip.length;

    public void Initialaze()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
    }

    public Coroutine StartShow()
    {
        return StartCoroutine(Show());
    }

    public Coroutine StartHide()
    {
        return StartCoroutine(Hide());
    }

    public void Unpause()
        => _pauseSpeed = 1;

    public void Pause()
        => _pauseSpeed = 0;

    private IEnumerator Show()
    {
        yield return StartCoroutine(FadeIn(1));
        _animator.SetBool(Constants.IsSkippingParameter, true);
    }

    private IEnumerator Hide()
    {
        yield return StartCoroutine(FadeIn(0));
        _animator.SetBool(Constants.IsSkippingParameter, false);
    }

    private IEnumerator FadeIn(float target)
    {
        while (_canvasGroup.alpha != target)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime * _fadeInSpeed * _pauseSpeed);
            yield return null;
        }
    }
}
