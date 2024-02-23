using DG.Tweening;
using System;
using System.Collections;
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
    [SerializeField] private FocusHandler _focusHandler;

    private float _fullProgress = 1;
    private float _minPitch = .5f;
    private Image _image;
    private Coroutine _coroutine;
    private bool _canFill = true;

    public bool WasFilling { get; private set; }

    public event Action ProgressBarFilled;

    public void Initialize() => _image = GetComponent<Image>();

    private void OnEnable()
    {
        _focusHandler.FocusChaned += OnFocusChanged;
    }

    private void OnDisable()
    {
        _focusHandler.FocusChaned -= OnFocusChanged;
    }

    public void ChangeFilling()
    {
        if (!_canFill)
            return;

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

    private void OnFocusChanged(bool isFocused)
    {
        if (isFocused == false && _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _source.Stop();
            _image.fillAmount = 0;
            WasFilling = false;
        }

        _canFill = isFocused;
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

            if (value == _fullProgress && _source.pitch < .8f)
                _source.pitch += _fillSpeed  * Time.deltaTime;
            else if (value != _fullProgress && _source.pitch > _minPitch)
                _source.pitch -= _fillSpeed * Time.deltaTime;

            if (_image.fillAmount == _fullProgress)
            {
                _source.Stop();
                _image.enabled = false;
                ProgressBarFilled?.Invoke();
            }

            yield return null;
        }

        _source.Stop();
        _coroutine = null;
    }
}
