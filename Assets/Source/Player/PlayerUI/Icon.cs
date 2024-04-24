using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class Icon : MonoBehaviour
{
    [SerializeField] private float _shakingDuration;
    [SerializeField] private float _shakingStrength;
    [SerializeField] private int _shakingVibrato;
    [SerializeField] private float _shakingRandomness;
    [SerializeField] private float _shakingDelay;

    private Image _image;
    private Color _startColor;
    private Color _disableColor;
    private RectTransform _rectTransform;

    private Sequence _shakingSequence;

    public void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _startColor = _image.color;
        _disableColor = new (_startColor.r, _startColor.g, _startColor.b, .5f);
    }

    public void PlayShakeEffect()
    {
        if (_shakingSequence != null)
            return;

        _shakingSequence = DOTween.Sequence();
        _shakingSequence.Append(_rectTransform.DOShakeAnchorPos(_shakingDuration, _shakingStrength, _shakingVibrato, _shakingRandomness)).AppendInterval(_shakingDelay).SetLoops(-1);
    }

    public void StopShaking()
    {
        if (_shakingSequence != null)
            _shakingSequence.Kill();

        _shakingSequence = null;
    }

    public void ChangeSprite(Sprite sprite) => _image.sprite = sprite;

    public void Interactable(bool isInteractable) => _image.color = isInteractable == true ? _startColor : _disableColor;
}
