using System.Collections.Generic;
using Source.Player.Abiilities.SceneEffectChangers;

namespace Source.Player
{
    public interface ISceneParticlesInfluencer
    {
        public void AddSceneEffectsToChange(
            List<EffectChangeHandler> effects,
            List<SoundChangeHandler> sounds,
            List<SplineChangeHandler> splines,
            List<AnimationChangeHandler> animations);
    }
}
