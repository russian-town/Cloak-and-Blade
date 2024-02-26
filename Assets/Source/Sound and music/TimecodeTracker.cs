using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class TimecodeTracker : MonoBehaviour, IPauseHandler
{
    private AudioSource _audioSource;
    private float _timecode;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    public void SetPause(bool isPause)
    {
        if (isPause)
            CashTimecode();
        else
            SetTimecode();
    }

    private void CashTimecode() => _timecode = _audioSource.time;

    private void SetTimecode() => _audioSource.time = _timecode;
}
