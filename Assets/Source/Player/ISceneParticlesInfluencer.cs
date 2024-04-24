using System.Collections.Generic;

public interface ISceneParticlesInfluencer
{
    public void AddSceneEffectsToChange(
        List<EffectChangeHandler> effects,
        List<SoundChangeHandler> sounds,
        List<SplineChangeHandler> splines,
        List<AnimationChangeHandler> animations);
}
