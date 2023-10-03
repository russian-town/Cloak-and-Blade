using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _sellButton;

    private Character _character;

    public event UnityAction<Character, CharacterView> SellButtonClicked;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.AddListener(TryLockCharacter);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.RemoveListener(TryLockCharacter);
    }

    public void Initialize(Sprite icon, int price, Character character)
    {
        _image.sprite = icon;
        _priceText.text = price.ToString();
        _character = character;
    }

    private void TryLockCharacter()
    {
        if (_character.IsBuyed)
            _sellButton.interactable = false;
    }
}
