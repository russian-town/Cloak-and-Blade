using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Knob : MonoBehaviour
{
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _focusedButtonColor;
    [SerializeField] private float _focusScale;
    [SerializeField] private float _changeFocusScale;
    [SerializeField] private float _unfocusSpeed;

    private Image _image;
    private Button _button;
    private ScrollIndicator _scrollIndicator;


    private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

    public void Initialize(ScrollIndicator scrollIndicator)
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _scrollIndicator = scrollIndicator;
        _button.onClick.AddListener(OnButtonClicked);
    }

    public void Focuse()
    {
        _image.color = _focusedButtonColor;
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_focusScale, _focusScale), _unfocusSpeed);
    }

    public void Unfocuse()
    {
        _image.color = _defaultButtonColor;
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_changeFocusScale, _changeFocusScale), _unfocusSpeed);
    }

    private void OnButtonClicked() => _scrollIndicator.WhichButtonClicked(this);
}
