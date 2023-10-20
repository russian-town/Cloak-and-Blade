using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TheWorld))]
public class YellowGhost : Player, ISceneParticlesInfluencer
{
    private TheWorld _theWorld;

    public event UnityAction ActionCompleted;

    protected override Command AbilityCommand()
    {
        _theWorld = GetComponent<TheWorld>();
        return new TheWorldCommand(_theWorld, this);
    }
}
