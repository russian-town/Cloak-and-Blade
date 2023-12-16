using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransformationPropParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    private void PlayGlowProp()
    {
        _particle.transform.rotation = transform.rotation;
        _particle.Play();
    } 
}
