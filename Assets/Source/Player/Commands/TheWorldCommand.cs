using System.Collections;
using System.Linq;
using UnityEngine;

public class TheWorldCommand : AbilityCommand
{
    private TheWorld _theWorld;
    private YellowGhost _yellowGhost;

    public TheWorldCommand(TheWorld theWorld, CommandExecuter executer, YellowGhost yellowGhost) : base(theWorld, executer)
    {
        _theWorld = theWorld;
        _yellowGhost = yellowGhost;
        _theWorld.AddSceneParticles(_yellowGhost.SceneEffects.ToList());
    }

    protected override IEnumerator WaitOfExecute()
    {
        yield break;
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return new WaitUntil(() => _theWorld.Cast(null));
        yield break;
    }

    protected override IEnumerator PrepareAction()
    {
        _theWorld.Prepare();
        yield break;
    }

    protected override void Cancel()
    {
        base.Cancel();
        _theWorld.Cancel();
    }
}
