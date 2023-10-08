using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private TMP_Text _selectText;

    private Character _character;

    public event UnityAction<Character, CharacterView> SellButtonClicked;
    public event UnityAction<Character, CharacterView> SelectButtonClicked;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.AddListener(TryLockBuyCharacter);
        _selectButton.onClick.AddListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.AddListener(TryLockSelectCharacter);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.RemoveListener(TryLockBuyCharacter);
        _selectButton.onClick.RemoveListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.RemoveListener(TryLockSelectCharacter);
    }

    public void Render(Sprite icon, int price, Character character)
    {
        _image.sprite = icon;
        _priceText.text = price.ToString();
        _character = character;
        _sellButton.gameObject.SetActive(true);
        _selectButton.gameObject.SetActive(false);
    }

    public void UpdateView()
    {
        TryLockBuyCharacter();
        TryLockSelectCharacter();
    }

    private void TryLockBuyCharacter()
    {
        if (_character.IsBuyed)
        {
            _sellButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(true);
            _sellButton.interactable = false;
        }
    }

    private void TryLockSelectCharacter()
    {
        if (_character.IsSelect)
        {
            _selectButton.interactable = false;
            _selectText.text = "Selected";
        }
        else
        {
            _selectButton.interactable = true;
            _selectText.text = "Select";
        }
    }
}
