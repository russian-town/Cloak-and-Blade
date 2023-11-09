using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TheWorld))]
public class YellowGhost : Player, ISceneParticlesInfluencer
{
    private TheWorld _theWorld;
    private List<EffectChangeHanldler> _effects = new List<EffectChangeHanldler>();

    public IReadOnlyList<EffectChangeHanldler> SceneEffects => _effects;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard, RewardedAdHandler adHandler)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler);
        _theWorld = GetComponent<TheWorld>();
        _theWorld.Initialize(UpgradeSetter);
    }

    public void AddSceneParticles(List<EffectChangeHanldler> effects) => _effects.AddRange(effects);

    protected override AbilityCommand AbilityCommand()
    {
        return new TheWorldCommand(_theWorld, CommandExecuter, this);
    }
}
