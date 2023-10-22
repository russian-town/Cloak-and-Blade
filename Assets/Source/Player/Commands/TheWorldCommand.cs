using System.Collections;
using System.Linq;
using UnityEngine;

public class TheWorldCommand : AbilityCommand
{
    private TheWorld _theWorld;
    private Coroutine _executeCoroutine;
    private CommandExecuter _executer;
    private YellowGhost _yellowGhost;

    public TheWorldCommand(TheWorld theWorld, CommandExecuter executer, YellowGhost yellowGhost) : base(theWorld)
    {
        _theWorld = theWorld;
        _theWorld.Initialize();
        _yellowGhost = yellowGhost;
        _theWorld.AddSceneParticles(_yellowGhost.SceneEffects.ToList());
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
