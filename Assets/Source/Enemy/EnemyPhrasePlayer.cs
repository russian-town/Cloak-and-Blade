using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhrasePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _stopRightThere;
    [SerializeField] private AudioClip _thenPayWithYourBlood;

    public void StopRightThere()
    {
        _source.clip = _stopRightThere;
        _source.Play();
    }

    public void ThenPayWithYourBlood()
    {
        _source.clip = _thenPayWithYourBlood;
        _source.Play();
    }
}
