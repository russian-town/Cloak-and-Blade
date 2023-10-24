using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Icon : MonoBehaviour
{
    private Image _image;
    private Color _startColor;
    private Color _disableColor;

    public void Initialize()
    {
        _image = GetComponent<Image>();
        _startColor = _image.color;
        _disableColor = new(_startColor.r, _startColor.g, _startColor.b, .5f);
    }

    public void ChangeSprite(Sprite sprite) => _image.sprite = sprite;

    public void Interactable(bool isInteractable) => _image.color = isInteractable == true ? _startColor : _disableColor;
}
