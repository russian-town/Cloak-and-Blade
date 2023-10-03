using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private readonly float _fadeInSpeed = 2f;

    private Player _player;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;
    private MonoBehaviour _context;
    private Coroutine _waitForEnemies;
    private PlayerAnimationHandler _playerAnimationHandler;
    private AnimationClip _hourglassClip;

    public SkipCommand(Player player, Animator animator, MonoBehaviour context, CanvasGroup hourglass, Coroutine waitForEnemies, PlayerAnimationHandler animationHandler, AnimationClip hourglassClip)
    {
        _player = player;
        _hourglassAnimator = animator;
        _context = context;
        _hourglass = hourglass;
        _waitForEnemies = waitForEnemies;
        _playerAnimationHandler = animationHandler;
        _hourglassClip = hourglassClip;
    }

    protected override IEnumerator PrepareAction()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return _context.StartCoroutine(FadeIn(1));
        _hourglassAnimator.SetBool(Constants.IsSkippingParameter, true);
        _playerAnimationHandler.PlaySkipAnimation();
        _player.SkipTurn();
        yield return _waitForEnemies;
        yield return new WaitForSeconds(_hourglassClip.length);
        yield return _context.StartCoroutine(FadeIn(0));
        _hourglassAnimator.SetBool(Constants.IsSkippingParameter, false);
        _playerAnimationHandler.StopSkipAnimation();
        Cancel();
        yield break;
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
