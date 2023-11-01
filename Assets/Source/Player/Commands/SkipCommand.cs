using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private Coroutine _waitForEnemies;
    private PlayerAnimationHandler _playerAnimationHandler;
    private Hourglass _hourglass;

    public SkipCommand(Player player, Coroutine waitForEnemies, PlayerAnimationHandler animationHandler, Hourglass hourglass, CommandExecuter executer) : base(executer)
    {
        _player = player;
        _waitForEnemies = waitForEnemies;
        _playerAnimationHandler = animationHandler;
        _hourglass = hourglass;
    }

    protected override IEnumerator PrepareAction()
    {
        yield break;
    }

    protected override IEnumerator WaitOfExecute()
    {
        yield break;
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return _hourglass.StartShow();
        _playerAnimationHandler.PlaySkipAnimation();
        _player.SkipTurn();
        yield return _waitForEnemies;
        yield return new WaitForSeconds(_hourglass.AnimationLength);
        _playerAnimationHandler.StopSkipAnimation();
        yield return _hourglass.StartHide();
        yield break;
    }

    protected override void OnCommandChanged(Command command) { return; }
}
