using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _priceText;

    public void Initialize(Sprite icon, int price)
    {
        _image.sprite = icon;
        _priceText.text = price.ToString();
    }
}
