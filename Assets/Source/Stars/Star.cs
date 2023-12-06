using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarAnimationHandler))]
public class Star : MonoBehaviour
{
    public StarAnimationHandler AnimationHandler { get; private set; }

    private void Awake()
    {
        AnimationHandler = GetComponent<StarAnimationHandler>();
    }

    public void PlayStarAppear() => AnimationHandler.PlayAppearAnimation();
}
