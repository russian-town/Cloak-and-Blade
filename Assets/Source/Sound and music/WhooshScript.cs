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

    public void PlayWhoosh()
    {
        if (_whooshCoroutine == null)
        {
            _isPlaying = true;
            _whooshCoroutine = StartCoroutine(WhooshCoroutine());
        }
    }

    private IEnumerator WhooshCoroutine()
    {
        while (_isPlaying)
        {
            if (!_isSourceTwo)
            {
                _waitDelay = new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
                _sourceOne.pitch = Random.Range(_minPitch, _maxPitch);
                _sourceOne.panStereo = Random.Range(-1, 1);
                _sourceOne.Play();
                _isSourceTwo = true;
                yield return _waitDelay;
                _waitDelay = null;
            }
            else
            {
                _waitDelay = new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
                _sourceTwo.pitch = Random.Range(_minPitch, _maxPitch);
                _sourceTwo.panStereo = Random.Range(-1, 1);
                _sourceTwo.Play();
                _isSourceTwo = false;
                yield return _waitDelay;
                _waitDelay = null;
            }
        }

        _whooshCoroutine = null;
    }
}
