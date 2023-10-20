using System.Collections;
using UnityEngine;

public class TheWorldCommand : Command
{
    private TheWorld _theWorld;
    private Player _player;
    private Coroutine _executeCoroutine;

    public TheWorldCommand(TheWorld theWorld, Player player)
    {
        _theWorld = theWorld;
        _theWorld.Initialize();
        _player = player;
    }

    public override IEnumerator WaitOfExecute()
    {
        _executeCoroutine = _player.StartCoroutine(Execute(null, _player));
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
