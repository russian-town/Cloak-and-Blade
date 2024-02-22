using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulStopper : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void StopParticle() => _particleSystem.Stop();
}
