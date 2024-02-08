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
    [SerializeField] private float _changeFocusScale;
    [SerializeField] private float _changeFocusSpeed;
    [SerializeField] private Image _focusImage;
    [SerializeField] private AudioClip _clip;

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
        _image.color = _focusedButtonColor;
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_focusScale, _focusScale), _changeFocusSpeed);
        _focusImage.DOColor(_focusImageColor, .7f);
        _source.PlayOneShot(_clip);
        print("poop");
    }

    public void Unfocus()
    {
        _image.color = _defaultButtonColor;
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_changeFocusScale, _changeFocusScale), _changeFocusSpeed);
        _focusImage.DOColor(new Color(_focusImageColor.r, _focusImageColor.g, _focusImageColor.b, 0), .6f);
    }

    private void OnButtonClicked() => _scrollIndicator.KnobClicked(this);
}
