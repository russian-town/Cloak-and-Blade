using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image), typeof(Button))]
public class Knob : MonoBehaviour
{
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _focusedButtonColor;
    [SerializeField] private Color _focusImageColor;
    [SerializeField] private float _focusScale;
    [SerializeField] private float _unFocusScale;
    [SerializeField] private float _changeFocusDuration;
    [SerializeField] private Image _focusImage;
    [SerializeField] private AudioClip _clip;

    private bool _isFocused;
    private bool _isUnFocused;
    private AudioSource _source;
    private Image _image;
    private Button _button;
    private ScrollIndicator _scrollIndicator;

    private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

    public void Initialize(ScrollIndicator scrollIndicator, AudioSource source)
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _scrollIndicator = scrollIndicator;
        _source = source;
        _button.onClick.AddListener(OnButtonClicked);
    }

    public void Focus()
    {
        if (_isFocused)
            return;

        _image.color = _focusedButtonColor;
        transform.DOScale(_focusScale, _changeFocusDuration);
        _focusImage.DOColor(_focusImageColor, .5f);
        _source.PlayOneShot(_clip);
        _isFocused = true;
        _isUnFocused = false;
    }

    public void Unfocus()
    {
        if (_isUnFocused)
            return;

        _image.color = _defaultButtonColor;
        transform.DOScale(_unFocusScale, _changeFocusDuration);
        _focusImage.DOColor(new Color(_focusImageColor.r, _focusImageColor.g, _focusImageColor.b, 0), .6f);
        _isUnFocused = true;
        _isFocused = false;
    }

    private void OnButtonClicked() => _scrollIndicator.KnobClicked(this);
}
