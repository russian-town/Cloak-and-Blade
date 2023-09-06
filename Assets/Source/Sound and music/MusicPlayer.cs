using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _calmMusic;
    [SerializeField] private AudioClip _battleMusic;

    private void Start()
    {
        _source.clip = _calmMusic;
        _source.Play();
    }

    public void SwitchMusic()
    {
        if (_source.clip == _calmMusic)
        {
            _source.clip = _battleMusic;
            _source.Play();
        }
        else if (_source.clip == _battleMusic)
        {
            _source.clip = _calmMusic;
            _source.Play();
        }
    }
}
