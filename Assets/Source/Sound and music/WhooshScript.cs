using System.Collections;
using UnityEngine;

public class WhooshScript : MonoBehaviour
{
    [SerializeField] private AudioSource _sourceOne;
    [SerializeField] private AudioSource _sourceTwo;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    private WaitForSeconds _waitDelay;
    private Coroutine _whooshCoroutine;
    private bool _isPlaying;
    private bool _isSourceTwo;

    public void StartWhooshCoroutine()
    {
        if (_whooshCoroutine == null)
        {
            _isPlaying = true;
            _whooshCoroutine = StartCoroutine(WhooshCoroutine());
        }
    }

    private void PlayWhooshSound(AudioSource whooshSource)
    {
        _waitDelay = new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        whooshSource.pitch = Random.Range(_minPitch, _maxPitch);
        whooshSource.panStereo = Random.Range(-1, 1);
        whooshSource.Play();
    }

    private IEnumerator WhooshCoroutine()
    {
        while (_isPlaying)
        {
            if (!_isSourceTwo)
            {
                PlayWhooshSound(_sourceOne);
                _isSourceTwo = true;
            }
            else
            {
                PlayWhooshSound(_sourceTwo);
                _isSourceTwo = false;
            }

            yield return _waitDelay;
            _waitDelay = null;
        }

        _whooshCoroutine = null;
    }
}
