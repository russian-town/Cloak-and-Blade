using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private AnimationClip _hourglassAnimation;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;
    private MonoBehaviour _context;
    private readonly float _fadeInSpeed = 2f;

    public SkipCommand(Player player, AnimationClip hourglassAnimation, Animator animator, MonoBehaviour context, CanvasGroup hourglass)
    {
        _player = player;
        _hourglassAnimation = hourglassAnimation;
        _hourglassAnimator = animator;
        _context = context;
        _hourglass = hourglass;
    }

    public override void Cancel()
    {
    }

    protected override IEnumerator PrepareAction()
    {
        yield return _context.StartCoroutine(FadeIn(1));
        _hourglassAnimator.SetBool(Constants.IsSkippingParametr, true);
        yield return new WaitForSeconds(_hourglassAnimation.length);
        yield return _context.StartCoroutine(FadeIn(0));
        _hourglassAnimator.SetBool(Constants.IsSkippingParametr, false);
        _player.SkipTurn();
        yield return null;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return null;
    }

    private IEnumerator FadeIn(float target)
    {
        while(_hourglass.alpha != target) 
        {
            _hourglass.alpha = Mathf.MoveTowards(_hourglass.alpha, target, Time.deltaTime * _fadeInSpeed);
            yield return null;
        }
    }
}
