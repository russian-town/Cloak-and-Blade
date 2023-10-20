using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private Coroutine _waitForEnemies;
    private PlayerAnimationHandler _playerAnimationHandler;
    private Hourglass _hourglass;
    private CommandExecuter _executer;

    public SkipCommand(Player player, Coroutine waitForEnemies, PlayerAnimationHandler animationHandler, Hourglass hourglass, CommandExecuter executer)
    {
        _player = player;
        _waitForEnemies = waitForEnemies;
        _playerAnimationHandler = animationHandler;
        _hourglass = hourglass;
        _executer = executer;
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
        Cancel(_executer);
        _executer.ResetCommand();
        yield break;
    }

    public override IEnumerator WaitOfExecute()
    {
        yield return _executer.StartCoroutine(Execute(null, _executer));
    }
}
