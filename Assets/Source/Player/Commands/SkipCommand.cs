using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private Coroutine _waitForEnemies;
    private PlayerAnimationHandler _playerAnimationHandler;

    public SkipCommand(Player player, Coroutine waitForEnemies, PlayerAnimationHandler animationHandler, CommandExecuter executer) : base(executer)
    {
        _player = player;
        _waitForEnemies = waitForEnemies;
        _playerAnimationHandler = animationHandler;
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
        _playerAnimationHandler.PlaySkipAnimation();
        _player.SkipTurn();
        yield return _waitForEnemies;
        _playerAnimationHandler.StopSkipAnimation();
        yield break;
    }

    protected override void OnCommandChanged(Command command) { return; }
}
