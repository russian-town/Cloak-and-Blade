using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private ISceneParticlesInfluencer _sceneParticlesInfluencer;

    public void Initialize(ISceneParticlesInfluencer sceneParticlesInfluencer)
    {
        if (sceneParticlesInfluencer == null)
            return;

        sceneParticlesInfluencer.ActionCompleted += OnActionCompleted;
    }

    private void OnActionCompleted()
    {
        FreezeEffects();
    }

    private void FreezeEffects()
    {
        _particleSystem.Stop();
    }
}
