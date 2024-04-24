using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TimecodeTracker : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _timecode;

    private void Awake()
        => _audioSource = GetComponent<AudioSource>();

    public void CashTimecode()
        => _timecode = _audioSource.time;

    public void SetTimecode()
        => _audioSource.time = _timecode;
}
