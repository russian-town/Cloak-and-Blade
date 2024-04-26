using System.Collections;
using UnityEngine;

public class SkipCommand : Command
{
    private Player _player;
    private IEnemyTurnWaiter _enemyTurnWaiter;
    private PlayerAnimationHandler _playerAnimationHandler;

    public SkipCommand(Player player, IEnemyTurnWaiter enemyTurnWaiter, PlayerAnimationHandler animationHandler, CommandExecuter executer)
        : base(executer)
    {
        _player = player;
        _enemyTurnWaiter = enemyTurnWaiter;
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

        if (_enemyTurnWaiter != null)
            yield return _enemyTurnWaiter.WaitForEnemies();

        _playerAnimationHandler.StopSkipAnimation();
        yield break;
    }

    protected override void OnCommandChanged(Command command)
    {
        return;
    }
}
