using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarAnimationHandler))]
public class Star : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private float _waitForSoundTime;

    private WaitForSeconds _waitTime;

    public StarAnimationHandler AnimationHandler { get; private set; }

    private void Awake()
    {
        AnimationHandler = GetComponent<StarAnimationHandler>();
        _waitTime = new WaitForSeconds(_waitForSoundTime);
    }

    public void PlayStarAppear() 
    {
        AnimationHandler.PlayAppearAnimation();
        StartCoroutine(PlaySoundWithPause());
    } 

    private IEnumerator PlaySoundWithPause()
    {
        yield return _waitTime;
        _source.Play();
    }
}
