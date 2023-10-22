using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TheWorld))]
public class YellowGhost : Player, ISceneParticlesInfluencer
{
    [SerializeField] private UpgradeSetter _upgradeSetter;

    private TheWorld _theWorld;
    private List<EffectChangeHanldler> _effects = new List<EffectChangeHanldler>();

    public IReadOnlyList<EffectChangeHanldler> SceneEffects => _effects;

    public override void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView, Gameboard gameboard)
    {
        base.Initialize(startCell, hourglass, enemyTurnHandler, playerView, gameboard);
        _theWorld = GetComponent<TheWorld>();
        _theWorld.Initialize(_upgradeSetter);
    }

    public void AddSceneParticles(List<EffectChangeHanldler> effects) => _effects.AddRange(effects);

    protected override AbilityCommand AbilityCommand()
    {
        return new TheWorldCommand(_theWorld, CommandExecuter, this);
    }
}
