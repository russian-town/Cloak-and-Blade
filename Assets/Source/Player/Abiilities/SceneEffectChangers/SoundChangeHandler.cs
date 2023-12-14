using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundChangeHandler : MonoBehaviour
{
    private AudioSource _source;
    private Coroutine _changePitchOverTime;

    public float InitialPitch { get; private set; }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        InitialPitch = _source.pitch;
    }

    public void ChangeAudioPitch(float value, float duration)
    {
        if (_changePitchOverTime != null)
            return;

        _changePitchOverTime = StartCoroutine(ChangeAudioPitchOverTime(value, duration));
    }

    private IEnumerator ChangeAudioPitchOverTime(float value, float duration)
    {
        while (_source.pitch != value)
        {
            _source.pitch = Mathf.MoveTowards(_source.pitch, value, duration * Time.deltaTime);
            yield return null;
        }

        _changePitchOverTime = null;
    }
}
