using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneScenario : MonoBehaviour
{
    [SerializeField] private float _flyingToTableWait;
    [SerializeField] private float _candleLightWait;
    [SerializeField] private float _textAppearWait;
    [SerializeField] private float _narratorWait;
    [SerializeField] private GameObject _candle;
    [SerializeField] private AudioSource _candleSound;
    [SerializeField] private Light _candleLight;
    [SerializeField] private float _candleLightFadeSpeed;
    [SerializeField] private float _candleIntensity;
    [SerializeField] private AudioClip _candleLoop;

    private WaitForSeconds _genericWait;

    void Start()
    {
        _candleLight.intensity = 0;
        _genericWait = new WaitForSeconds(_flyingToTableWait);
        StartCoroutine(CutsceneCoroutine());
    }

    private IEnumerator CutsceneCoroutine()
    {
        yield return _genericWait;
        print("im at table");
        _genericWait = new WaitForSeconds(_candleLightWait);
        yield return _genericWait;
        _candle.SetActive(true);
        _candleSound.Play();

        while (_candleLight.intensity != _candleIntensity)
        {
            _candleLight.intensity = Mathf.MoveTowards(_candleLight.intensity, _candleIntensity, _candleLightFadeSpeed * Time.deltaTime);
            yield return null;
        }
        _genericWait = new WaitForSeconds(_textAppearWait);
        yield return _genericWait;
        _candleSound.clip = _candleLoop;
        _candleSound.loop = true;
        print("text started");
        _genericWait = new WaitForSeconds(_narratorWait);
        yield return _genericWait;
        print("narrator started speech");
    }
}
