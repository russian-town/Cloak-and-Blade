using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TheWorld))]
public class YellowGhost : Player, ISceneParticlesInfluencer
{
    private TheWorld _theWorld;
    private List<EffectChangeHanldler> _effects = new List<EffectChangeHanldler>();

    public IReadOnlyList<EffectChangeHanldler> SceneEffects => _effects;

    public void AddSceneParticles(List<EffectChangeHanldler> effects) => _effects.AddRange(effects);

    protected override AbilityCommand AbilityCommand()
    {
        _theWorld = GetComponent<TheWorld>();
        return new TheWorldCommand(_theWorld, CommandExecuter, this);
    }
}
