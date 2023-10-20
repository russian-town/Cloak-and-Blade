using System.Collections;
using UnityEngine;

public class TheWorldCommand : Command
{
    private TheWorld _theWorld;
    private Player _player;
    private Coroutine _executeCoroutine;
    private CommandExecuter _executer;

    public TheWorldCommand(TheWorld theWorld, Player player, CommandExecuter executer)
    {
        _theWorld = theWorld;
        _theWorld.Initialize();
        _player = player;
        _executer = executer;
    }

    public override IEnumerator WaitOfExecute()
    {
        _executeCoroutine = _executer.StartCoroutine(Execute(null, _executer));
        yield return _executeCoroutine;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return new WaitUntil(() => _theWorld.Cast(clickedCell));
        yield break;
    }

    protected override IEnumerator PrepareAction()
    {
        _theWorld.Prepare();
        yield break;
    }

    public override void Cancel(MonoBehaviour context)
    {
        base.Cancel(context);
        _theWorld.Cancel();
    }
}
