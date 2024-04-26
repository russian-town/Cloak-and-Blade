using System.Collections.Generic;
using Source.Gameboard.Cell;
using Source.Player.Abiilities;
using Source.Player.Abiilities.SceneEffectChangers;
using Source.Player.Commands;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.Hourglass;
using Source.Room;
using Source.Yandex;
using UnityEngine;

namespace Source.Player.YellowGhost
{
    [RequireComponent(typeof(TheWorld))]
    public class YellowGhost : Player, ISceneParticlesInfluencer
    {
        private TheWorld _theWorld;
        private List<EffectChangeHandler> _effects = new List<EffectChangeHandler>();
        private List<SoundChangeHandler> _soundList = new List<SoundChangeHandler>();
        private List<SplineChangeHandler> _splineList = new List<SplineChangeHandler>();
        private List<AnimationChangeHandler> _animationList = new List<AnimationChangeHandler>();

        public IReadOnlyList<EffectChangeHandler> SceneEffects => _effects;

        public IReadOnlyList<SoundChangeHandler> SceneSounds => _soundList;

        public IReadOnlyList<SplineChangeHandler> SceneSplines => _splineList;

        public IReadOnlyList<AnimationChangeHandler> SceneAnimations => _animationList;

        public override void Initialize(
            Cell startCell,
            Hourglass hourglass,
            IEnemyTurnWaiter enemyTurnHandler,
            Gameboard.Gameboard gameboard,
            RewardedAdHandler adHandler,
            PlayerView playerView,
            Battery battery)
        {
            base.Initialize(startCell, hourglass, enemyTurnHandler, gameboard, adHandler, playerView, battery);
            _theWorld = GetComponent<TheWorld>();
            _theWorld.Initialize(UpgradeSetter, playerView);
        }

        public void AddSceneEffectsToChange(
            List<EffectChangeHandler> effects,
            List<SoundChangeHandler> sounds,
            List<SplineChangeHandler> splines,
            List<AnimationChangeHandler> animations)
        {
            _effects.AddRange(effects);
            _soundList.AddRange(sounds);
            _splineList.AddRange(splines);
            _animationList.AddRange(animations);
        }

        public override AbilityCommand GetAbilityCommand()
        {
            return new TheWorldCommand(_theWorld, CommandExecuter, this);
        }
    }
}
