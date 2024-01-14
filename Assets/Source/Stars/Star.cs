using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarAnimationHandler))]
public class Star : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public StarAnimationHandler AnimationHandler { get; private set; }

    private void Awake()
    {
        AnimationHandler = GetComponent<StarAnimationHandler>();
    }

    public void PlayStarAppear() 
    {
        AnimationHandler.PlayAppearAnimation();
        _source.Play();
    } 
}
