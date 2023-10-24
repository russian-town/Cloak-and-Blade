using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class Hourglass : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _fadeInSpeed;
    [SerializeField] private AnimationClip _hourglassClip;

    private CanvasGroup _canvasGroup;
    private Animator _animator;
    private CommandExecuter _commandExecuter;
    private float _pauseSpeed = 1;

    public float AnimationLength => _hourglassClip.length;

    public void Initialaze(CommandExecuter commandExecuter)
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
        _commandExecuter = commandExecuter;
    }

    public Coroutine StartShow()
    {
        return _commandExecuter.StartCoroutine(Show());
    }

    public Coroutine StartHide()
    {
        return _commandExecuter.StartCoroutine(Hide());
    }

    private IEnumerator Show()
    {
        yield return _commandExecuter.StartCoroutine(FadeIn(1));
        _animator.SetBool(Constants.IsSkippingParameter, true);
    }

    private IEnumerator Hide()
    {
        yield return _commandExecuter.StartCoroutine(FadeIn(0));
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

    public void SetPause(bool isPause) => _pauseSpeed = isPause ? 0 : 1;
}
