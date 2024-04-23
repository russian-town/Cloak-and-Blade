using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderMaker : MonoBehaviour
{
    [SerializeField] private Light _thunderLight;
    [SerializeField] private List<AudioClip> _thunderSounds;
    [SerializeField] private AudioSource _source;
    [SerializeField] private float _lightIntensity;
    [SerializeField] private float _pauseBetweenThunder;
    [SerializeField] private float _possibleMaxStartDelay;
    [SerializeField] private float _possibleMinStartDelay = 0;
    [SerializeField] private float _minLightLength;
    [SerializeField] private float _maxLightLength;
    [SerializeField] private float _minLightDelay;
    [SerializeField] private float _maxLightDelay;
    [SerializeField] private float _minSoundDelay;
    [SerializeField] private float _maxSoundDelay;
    [SerializeField] private int _minThunderStreak;
    [SerializeField] private int _maxThunderStreak;
    [SerializeField] private float _fadeAwayTime;
    [SerializeField] private bool _playOnAwake;

    private float _timePassed;
    private float _baseLightIntensity;
    private int _thundersToStrike;
    private bool _soundPlayed;
    private Coroutine _thunderCoroutine;
    private WaitForSeconds _startDelay;
    private WaitForSeconds _lightLength;
    private WaitForSeconds _lightDelay;
    private WaitForSeconds _soundDelay;

    private void Start()
    {
        _baseLightIntensity = _thunderLight.intensity;

        if (_playOnAwake)
            _thunderCoroutine = StartCoroutine(ThunderCoroutine());
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed >= _pauseBetweenThunder)
        {
            _timePassed = 0;

            if (_thunderCoroutine == null)
                _thunderCoroutine = StartCoroutine(ThunderCoroutine());
        }
    }

    private IEnumerator ThunderCoroutine()
    {
        _startDelay = new WaitForSeconds(Random.Range(_possibleMinStartDelay, _possibleMaxStartDelay));
        _soundDelay = new WaitForSeconds(Random.Range(_minSoundDelay, _maxSoundDelay));
        _thundersToStrike = Random.Range(_minThunderStreak, _maxThunderStreak);

        yield return _startDelay;

        for (int i = 0; i < _thundersToStrike + 1; i++)
        {
            _thunderLight.intensity += _lightIntensity;
            _lightLength = new WaitForSeconds(Random.Range(_minLightLength, _maxLightLength));
            yield return _lightLength;
            _lightLength = null;

            if (i == _thundersToStrike)
            {
                while (_thunderLight.intensity != _baseLightIntensity)
                {
                    if(_soundPlayed == false)
                    {
                        yield return _soundDelay;
                        _source.PlayOneShot(_thunderSounds[Random.Range(0, _thunderSounds.Count)]);
                        _soundPlayed = true;
                    }

                    _thunderLight.intensity = Mathf.MoveTowards(_thunderLight.intensity, _baseLightIntensity, _fadeAwayTime * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                _thunderLight.intensity = _baseLightIntensity;
            }

            _lightDelay = new WaitForSeconds(Random.Range(_minLightDelay, _maxLightDelay));
            yield return _lightDelay;
            _lightDelay = null;
        }

        _soundDelay = null;
        _thunderCoroutine = null;
        _soundPlayed = false;
        _thundersToStrike = 0;
    }
}
