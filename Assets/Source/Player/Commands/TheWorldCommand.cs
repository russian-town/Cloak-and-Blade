using System.Collections;
using System.Linq;
using UnityEngine;

public class TheWorldCommand : AbilityCommand
{
    private TheWorld _theWorld;
    private YellowGhost _yellowGhost;

    public TheWorldCommand(TheWorld theWorld, CommandExecuter executer, YellowGhost yellowGhost)
        : base(theWorld, executer)
    {
        _theWorld = theWorld;
        _yellowGhost = yellowGhost;
        _theWorld.AddSceneEffectsToChange(
            _yellowGhost.SceneEffects.ToList(),
            _yellowGhost.SceneSounds.ToList(),
            _yellowGhost.SceneSplines.ToList(),
            _yellowGhost.SceneAnimations.ToList());
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
}
