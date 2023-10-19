using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private Coroutine _waitForEnemies;
    private PlayerAnimationHandler _playerAnimationHandler;
    private Hourglass _hourglass;

    public SkipCommand(Player player, Coroutine waitForEnemies, PlayerAnimationHandler animationHandler, Hourglass hourglass)
    {
        _player = player;
        _waitForEnemies = waitForEnemies;
        _playerAnimationHandler = animationHandler;
        _hourglass = hourglass;
    }

    protected override IEnumerator PrepareAction()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return _hourglass.StartShow();
        _playerAnimationHandler.PlaySkipAnimation();
        _player.SkipTurn();
        yield return _waitForEnemies;
        yield return new WaitForSeconds(_hourglass.AnimationLength);
        yield return _hourglass.StartHide();
        _playerAnimationHandler.StopSkipAnimation();
        Cancel();
        _player.ResetCommand();
        yield break;
    }
}
