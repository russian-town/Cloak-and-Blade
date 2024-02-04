using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBarFiller : MonoBehaviour
{
    [SerializeField] private float _fillSpeed;
    [SerializeField] private float _colorChangeDuration;
    [SerializeField] private AudioSource _source;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _filledColor;

    public event Action ProgressBarFilled;

    private float _fullProgress = 1;
    private float _minPitch = .5f;
    private Image _image;
    private Coroutine _coroutine;
    private Color _currentColor;

    public bool WasFilling { get; private set; }

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeFilling()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (WasFilling)
        {
            _coroutine = StartCoroutine(FillImage(0));
            WasFilling = false;
        }
        else
        {
            _coroutine = StartCoroutine(FillImage(_fullProgress));
            WasFilling = true;
        }
    }

    private IEnumerator FillImage(float value)
    {
        _source.Play();

        if(value == _fullProgress)
            _image.DOColor(_filledColor, _colorChangeDuration).SetEase(Ease.InSine);
        else
            _image.DOColor(_originalColor, _colorChangeDuration).SetEase(Ease.Unset);

        while (_image.fillAmount != value)
        {
            _image.fillAmount = Mathf.MoveTowards(_image.fillAmount, value, _fillSpeed * Time.deltaTime);

            if (value == _fullProgress && _source.pitch < 1)
                _source.pitch += _fillSpeed  * Time.deltaTime;
            else if (value != _fullProgress && _source.pitch > _minPitch)
                _source.pitch -= _fillSpeed * Time.deltaTime;

            if (_image.fillAmount == _fullProgress)
                ProgressBarFilled?.Invoke();

            yield return null;
        }

        _source.Stop();
        _coroutine = null;
    }
}
